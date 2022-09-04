using Amazon.CDK;
using Amazon.CDK.AWS.DynamoDB;
using CodingChallenge.Cdk.Extensions;
using CodingChallenge.Infrastructure;
using CodingChallenge.Infrastructure.Extensions;
using CodingChallenge.Infrastructure.Persistence.TVMazeRecord;

namespace CodingChallenge.Cdk.Stacks;

public sealed class DatabaseStack : Stack
{
    public const string arnSuffixValue = "arn";
    public DatabaseStack(Construct parent, string id, IStackProps props, AWSAppProject awsApplication) : base(parent, id, props)
    {
        SetupTableForTVMazeRecord(awsApplication);
    }

    public void SetupTableForTVMazeRecord(AWSAppProject awsApplication)
    {

        string dynamoDBTableFullName = awsApplication.GetDynamodbTableName(typeof(TVMazeRecordDataModel));
        var dynamoDbTableProps = new TableProps()
        {
            TableName = dynamoDBTableFullName
        };
        dynamoDbTableProps.PartitionKey = new Amazon.CDK.AWS.DynamoDB.Attribute()
        {
            Type = Amazon.CDK.AWS.DynamoDB.AttributeType.STRING,
            Name = nameof(Infrastructure.Persistence.TVMazeRecord.TVMazeRecordDataModel.TVMazeIndex)
        };

        // dynamoDbTableProps.SortKey = new Amazon.CDK.AWS.DynamoDB.Attribute()
        // {
        //     Type = Amazon.CDK.AWS.DynamoDB.AttributeType.STRING,
        //     Name = nameof(Infrastructure.Persistence.TVMazeRecord.TVMazeRecordDataModel.TVMazeIndex)
        // };
        var table = new Table(this, dynamoDBTableFullName, dynamoDbTableProps);
        awsApplication.SetCfOutput(this, $"{typeof(TVMazeRecordDataModel).Name.ToLower()}-{arnSuffixValue}", table.TableArn);
    }
}

