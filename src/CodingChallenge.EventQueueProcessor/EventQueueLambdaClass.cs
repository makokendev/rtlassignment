using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using CodingChallenge.Application;
using CodingChallenge.Application.NFT.Commands.Burn;
using CodingChallenge.Application.NFT.Commands.Mint;
using CodingChallenge.EventQueueProcessor.Logger;
using CodingChallenge.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace CodingChallenge.EventQueueProcessor;
public class EventQueueLambdaClass
{
    public ILogger logger;
    public IConfiguration configuration;
    public IServiceProvider serviceProvider;
    public AWSAppProject awsApplication;

    public EventQueueLambdaClass()
    {
        LoadConfiguration();
        SetupLogger();
        ConfigureServices(new ServiceCollection());
    }

    protected virtual void LoadConfiguration()
    {
        configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables().Build();
        awsApplication = new AWSAppProject();
        configuration.GetSection(Constants.APPLICATION_ENVIRONMENT_VAR_PREFIX).Bind(awsApplication);
    }
    protected virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddApplicationBaseDependencies();
        services.AddInfrastructureDependencies(configuration, logger);
        services.AddSingleton<ILogger>(logger);
        services.AddSingleton<AWSAppProject>(awsApplication);

        services.AddTransient<TVMazeScrapeCommandController, TVMazeScrapeCommandController>();
        services.AddTransient<TVMazeLambdaRunner, TVMazeLambdaRunner>();
        serviceProvider = services.BuildServiceProvider();
    }


    public void SetupLogger()
    {
        logger = new CustomLambdaLoggerProvider(new CustomLambdaLoggerConfig()
        {
            LogLevel = LogLevel.Debug,
            InfrastructureProject = awsApplication

        }).CreateLogger(nameof(EventQueueLambdaClass));
    }

    public async Task HandleAsync(SQSEvent sQSEvent, ILambdaContext context)
    {
        logger.LogInformation($"Handling SQS Event");
        if (sQSEvent == null || sQSEvent.Records == null || !sQSEvent.Records.Any())
        {
            logger.LogInformation($"No records are found");
            return;
        }
        var runner = serviceProvider.GetService<TVMazeLambdaRunner>();

        foreach (var record in sQSEvent.Records)
        {
            try
            {
                logger.LogInformation($"log debug {record.Body}");
                var taskObject = JsonConvert.DeserializeObject<AddScrapeTaskCommand>(record.Body);
                var startIndex = Convert.ToInt32(taskObject.StartIndex);
                var endIndex = Convert.ToInt32(taskObject.EndIndex);
                var tasks = new List<Task>();
                for (int i = startIndex; i <= endIndex; i++)
                {
                    logger.LogInformation($"sending scrape command for index {i}");
                    tasks.Add(runner.SendScrapeCommand(i));
                }
                await Task.WhenAll(tasks);
                var results = new List<ScrapeCommandResponse>();
                foreach (var task in tasks)
                {
                    var result = ((Task<ScrapeCommandResponse>)task).Result;
                    results.Add(result);
                    if (!result.IsSuccess && taskObject.TryCount < 6)
                    {
                        var newOrder = new AddScrapeTaskCommand(result.index,result.index,taskObject.TryCount+1);
                         logger.LogInformation($"Adding a new task for a failed task. Id -> {result.index}.Try Count -> {taskObject.TryCount}");
                        await runner.AddScrapeTaskAsync(newOrder);
                    }
                }
                //await Task.FromResult("");

            }

            catch (Exception ex)
            {
                logger.LogInformation($"error processing queue... Message: {ex.Message}. StackTrace {ex.StackTrace}. exception type -> {ex.GetType()}");
                logger.LogInformation($"inner exception error processing queue... Message: {ex.InnerException?.Message}. StackTrace {ex.InnerException?.StackTrace}");
                throw;
            }
        }
    }

}
