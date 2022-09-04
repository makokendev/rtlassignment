using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using CodingChallenge.Application;
using CodingChallenge.Application.TVMaze.Commands.Burn;
using CodingChallenge.Application.TVMaze.Commands.Mint;
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

    public const int TryCountLimit = 20;
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
                    logger.LogInformation($"{i} - sending scrape command for index {i}");
                    tasks.Add(runner.SendScrapeCommand(i));
                }
                await Task.WhenAll(tasks);
                var results = new List<ScrapeCommandResponse>();
                foreach (var task in tasks)
                {
                    var result = ((Task<ScrapeCommandResponse>)task).Result;
                    results.Add(result);
                    logger.LogInformation($"{result.index} - asyncresponse received");
                    if (result.IsSuccess && result.CastListEmpty == true)
                    {
                        logger.LogInformation($"{result.index} - success call && cast list is empty. Noted...");
                        //continue;
                    }
                    if (!result.IsSuccess && result.NotFound == true)
                    {
                        logger.LogInformation($"{result.index} - Item Not found. We won't try again");
                        continue;
                    }
                    if (!result.IsSuccess)
                    {
                        logger.LogInformation($"{result.index} -- NOT SUCCESS. Will retry if already not in db or try count not exceeded.");
                        if (taskObject.TryCount < TryCountLimit)
                        {
                            var indexResult = await runner.GetTokenByIdAsync(new Application.TVMaze.Queries.Token.GetTVMazeItemByIndexQuery(result.index.ToString()));
                            if (indexResult == null)
                            {
                                logger.LogInformation($"{result.index} -- Item is not in database");
                                var newOrder = new AddScrapeTaskCommand(result.index, result.index, taskObject.TryCount + 1);
                                logger.LogInformation($"{result.index} - Try Count -> {taskObject.TryCount} - Adding a new task for a failed task. Id -> {result.index}.");
                                await runner.AddScrapeTaskAsync(newOrder);
                            }
                            else
                            {
                                logger.LogInformation($"{result.index} -- Item already exists in database");
                            }
                        }else{
                            logger.LogInformation($"{result.index} -- Try Count limit exceeded.");
                        }
                    }
                    else
                    {
                        logger.LogInformation($"{result.index} -- Saved in DB!");
                    }
                }
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
