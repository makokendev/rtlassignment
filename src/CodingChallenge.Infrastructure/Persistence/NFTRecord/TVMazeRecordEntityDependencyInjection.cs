
using CodingChallenge.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CodingChallenge.Infrastructure.Persistence.TVMazeRecord;

public static class TVMazeRecordEntityDependencyInjection
{
    public static IServiceCollection AddNftEntityInfrastructure(this IServiceCollection services, IConfiguration configuration, ILogger logger)
    {
        services.AddScoped<ITVMazeRecordRepository, TVMazeRecordDynamoDBRepository>();
        // var databaseType = configuration.GetValue<string>(Constants.DATABASE_TYPE_ENV_VAR_KEY);
        // if (!string.IsNullOrWhiteSpace(databaseType)
        //         && configuration.GetValue<string>(Constants.DATABASE_TYPE_ENV_VAR_KEY).Equals(Constants.DATABASE_TYPE_DYNAMODB_ENV_VAR_KEY))
        // {
        //     logger.LogDebug("Using Dynamodb repository");
        //     services.AddScoped<ITVMazeRecordRepository, TVMazeRecordDynamoDBRepository>();
        // }
        // else
        // {
        //     logger.LogDebug("Using SQL LITE repository");
        //     services.AddScoped<ITVMazeRecordRepository, TVMazeRecordRepository>();
        // }
        return services;
    }
}
