using System.Threading.Tasks;
using CodingChallenge.Application;
using CodingChallenge.Infrastructure;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CodingChallenge.Console;

partial class Program
{
    static IConfiguration configuration;
    static ILogger logger;
    static ServiceProvider serviceProvider;

    static async Task Main(string[] args)
    {
        LoadConfiguration();
        SetupLogger();
        ConfigureServices(new ServiceCollection());
        var commandParser = serviceProvider.GetService<TVMazeConsoleRunner>();
        var parser = CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args);
        await parser.WithParsedAsync(async options => await commandParser.RunOptionsAsync(options));
        await parser.WithNotParsedAsync(async errs => await commandParser.HandleParseErrorAsync(errs));
    }
    static void LoadConfiguration()
    {
        configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables().Build();
    }

    static void ConfigureServices(IServiceCollection services)
    {
        services.AddApplicationBaseDependencies();
        services.AddInfrastructureDependencies(configuration,logger);
        services.AddSingleton<ILogger>(logger);
        services.AddTransient<TVMazeScrapeCommandController, TVMazeScrapeCommandController>();
        services.AddTransient<TVMazeConsoleRunner, TVMazeConsoleRunner>();
        serviceProvider = services.BuildServiceProvider();
    }

    static void SetupLogger()
    {
        var loggerFactory = LoggerFactory.Create(
                builder => builder
                            // add debug output as logging target
                            //.AddDebug()
                            .AddConsole()
                            // set minimum level to log
                            .SetMinimumLevel(LogLevel.Information));
        logger = loggerFactory.CreateLogger("CodingChallenge Console App");
    }

}