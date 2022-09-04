using System.Collections.Generic;
using Microsoft.Extensions.Logging;

using System.Threading.Tasks;
using CodingChallenge.Infrastructure.Extensions;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace CodingChallenge.Infrastructure.Repository;

public interface IApplicationDynamoDBBase<T> where T : class
{
    Task DeleteAsync(List<T> dataModelItems);
    Task<T> GetAsync(string id);
    Task SaveAsync(List<T> dataModelItems);
    Task UpdateAsync(List<T> dataModelItems);
}

public class ApplicationDynamoDBBase<T> : DynamoDBRepository, IApplicationDynamoDBBase<T> where T : class
{
    protected virtual DynamoDBOperationConfig DynamoDBOperationConfig { get; }

    public ApplicationDynamoDBBase(ILogger logger, AWSAppProject awsApplication) : base(logger, awsApplication)
    {
        DynamoDBOperationConfig = GetOperationConfig(AwsApplication.GetDynamodbTableName(typeof(T)));
    }

    public async Task DeleteAsync(List<T> dataModelItems)
    {
        var dataModelBatch = Context.CreateBatchWrite<T>(DynamoDBOperationConfig);
        dataModelBatch.AddDeleteItems(dataModelItems);
        await dataModelBatch.ExecuteAsync();
    }

    public async Task<T> GetAsync(string id)
    {
        var result = await Context.LoadAsync<T>(id, DynamoDBOperationConfig);
        return result;
    }


//  var getItemRequestTemplate = new
//         {
//             Key = new
//             {
//                 TVMazeIndex = new
//                 {
//                     S = "$method.request.querystring.id"
//                 },
//                 TVMazeType = new
//                 {
//                     S = "$method.request.querystring.tvmazetype"
//                 }
//             },
//             TableName = awsApplication.GetDynamodbTableName(typeof(TVMazeRecordDataModel))
//         };
//         var queryRequestTemplate = new Dictionary<string, object>{
//             {"TableName",awsApplication.GetDynamodbTableName(typeof(TVMazeRecordDataModel))},
//             {"KeyConditionExpression","TVMazeType = :c"},
//             {"ExpressionAttributeValues",new Dictionary<string,object>{
//                 {":c",new Dictionary<string,object>{
//                     {"S","$method.request.querystring.tvmazetype"}
//                 }}
//             }}
//         };


    public async Task<List<T>> GetBySortKeyAsync(string sortKeyField, string sortKeyValue)
    {

        return await Context.ScanAsync<T>(new ScanCondition[] { new ScanCondition(sortKeyField, ScanOperator.Equal, sortKeyValue) }, DynamoDBOperationConfig).GetRemainingAsync();
    }


    public async Task SaveAsync(List<T> dataModelItems)
    {
        //Logger.LogInformation($"ApplicationDynamoDBBase... SaveAsync");
        var dataModelBatch = Context.CreateBatchWrite<T>(DynamoDBOperationConfig);
        //Logger.LogInformation($"log information... item count {dataModelItems.Count}");
        dataModelBatch.AddPutItems(dataModelItems);
        await dataModelBatch.ExecuteAsync();
    }

    public async Task UpdateAsync(List<T> dataModelItems)
    {
        var dataModelBatch = Context.CreateBatchWrite<T>(DynamoDBOperationConfig);
        dataModelBatch.AddPutItems(dataModelItems);
        await dataModelBatch.ExecuteAsync();
    }

}
