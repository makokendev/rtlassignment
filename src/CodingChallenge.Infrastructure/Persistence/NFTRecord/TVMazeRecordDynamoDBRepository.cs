using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.Generic;
using CodingChallenge.Application.Interfaces;
using CodingChallenge.Application.Exceptions;
using CodingChallenge.Domain.Entities.NFT;
using CodingChallenge.Infrastructure.Repository;
using System;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using CodingChallenge.Infrastructure.Extensions;
using Newtonsoft.Json;

namespace CodingChallenge.Infrastructure.Persistence.TVMazeRecord;
public class TVMazeRecordDynamoDBRepository : ApplicationDynamoDBBase<TVMazeRecordDataModel>, ITVMazeRecordRepository
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly AmazonSimpleNotificationServiceClient _snsClient;
    //TODO - get from CDK project!
    public const string eventTopicSuffix = "eventtopic";

    private readonly string TopicArn;

    public TVMazeRecordDynamoDBRepository(IMapper mapper, ILogger logger, AWSAppProject awsApplication) : base(logger, awsApplication)
    {
        _mapper = mapper;
        this._logger = logger;
        _snsClient = new AmazonSimpleNotificationServiceClient();
        TopicArn = $"arn:aws:sns:us-east-1:476631482508:{awsApplication.GetResourceName(eventTopicSuffix)}";
    }

    public async Task ScrapeAsync(TVMazeRecordEntity nFTEntity)
    {
        _logger.LogDebug($"Mint repo action is being executed... Token Id is {nFTEntity.TokenId}");
        var dataModel = _mapper.Map<TVMazeRecordEntity, TVMazeRecordDataModel>(nFTEntity);

        await this.SaveAsync(new List<TVMazeRecordDataModel>(){
            dataModel
        });
        _logger.LogDebug($"Mint repo action is successfully executed ... Token Id is {nFTEntity.TokenId}");
    }

    public class AddScrapeTaskClass{
        public string StartIndex{get;set;}
        public string EndIndex{get;set;}
    }
    public async Task AddScrapeTaskAsync(string startIndex,string endIndex)
    {
        _logger.LogDebug($"Burn repo action is being executed... Token Id is {startIndex}");
        var publishRequest = new PublishRequest()
        {
            Message = JsonConvert.SerializeObject(new AddScrapeTaskClass(){
                StartIndex = startIndex,
                EndIndex = endIndex
            }),
            TopicArn = TopicArn
        };
        await _snsClient.PublishAsync(publishRequest);
        _logger.LogDebug($"Burn repo action is successfully executed... Token Id is {startIndex}");
    }

    public async Task<TVMazeRecordEntity> GetByTokenIdAsync(string tokenId)
    {
        _logger.LogDebug($"Get By Token repo action is being executed... Token Id is {tokenId}");
        var result = await this.GetAsync(tokenId);
        var mappedEntity = _mapper.Map<TVMazeRecordDataModel, TVMazeRecordEntity>(result);
        _logger.LogDebug($"Get By Token repo action is successfully executed for token with Id {tokenId}. Returning result");
        return mappedEntity;
    }

    public async Task<List<TVMazeRecordEntity>> GetByWalletIdAsync(string walletId)
    {
        _logger.LogDebug($"Get tokens from wallet repo action is being executed... Wallet Id is {walletId}");
        var results = await GetBySortKeyAsync(nameof(TVMazeRecordDataModel.WalletId), walletId);
        var mappedEntity = _mapper.Map<List<TVMazeRecordDataModel>, List<TVMazeRecordEntity>>(results);
        _logger.LogDebug($"Get tokens from wallet repo action is successfully executed for wallet with Id {walletId}. Returning result");
        return mappedEntity;
    }

    public async Task ResetAsync()
    {
        await Task.CompletedTask;
        throw new NotSupportedException("Reset Action is not supported at his point");
    }
    public async Task TransferAsync(TVMazeRecordEntity nFTEntity, string newWalletId)
    {
        _logger.LogDebug($"Transfer repo action is being executed... Token Id is {nFTEntity.TokenId}");
        var dataModel = _mapper.Map<TVMazeRecordEntity, TVMazeRecordDataModel>(nFTEntity);
        var token = await GetAsync(nFTEntity.TokenId);
        if (token == null)
        {
            throw new NFTTokenNotFoundException($"Token with id {nFTEntity.TokenId} does not exist in the database");
        }
        token.WalletId = newWalletId;
        await UpdateAsync(new List<TVMazeRecordDataModel>(){
            token
        });
        _logger.LogDebug($"Transfer repo action is successfully executed ... Token Id is {nFTEntity.TokenId}");
    }
}
