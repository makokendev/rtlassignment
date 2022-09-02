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
using RestSharp;
using CodingChallenge.Application.NFT.Commands.Burn;
using CodingChallenge.Domain.Entities;
using System.Net;
using System.Linq;

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
        _logger.LogInformation($"AWS OBJECT -> {JsonConvert.SerializeObject(awsApplication)}");
        TopicArn = $"arn:aws:sns:us-east-1:476631482508:{awsApplication.GetResourceName(eventTopicSuffix)}";
    }

    // Task<Domain.Entities.TVMazeCastHttpCallResponse> ITVMazeRecordRepository.GetTVMazeCastById(string id)
    // {
    //     throw new NotImplementedException();
    // }
    public async Task<Domain.Entities.TVMazeCastHttpCallResponse> GetTVMazeCastById(int id)
    {
        var retObj = new TVMazeCastHttpCallResponse();
        var response = await TVMazeCastByShowIdHttpGetCall(id);
        if (response == null)
        {
            retObj.IsSuccessful = false;
            _logger.LogInformation($"response--EXITING. {id} getting tv maze cast by id :{id} - SUCCESS");
            return retObj;
        }
        _logger.LogInformation($"{id} - StatusCode response code is {response.StatusCode}");
        _logger.LogInformation($"error mesage code is {response.ErrorMessage}");

        _logger.LogInformation($"response json is {JsonConvert.SerializeObject(response)}");

        if (response.IsSuccessful)
        {
            _logger.LogInformation($"response--SUCCESS. {id} getting tv maze cast by id :{id} - SUCCESS");
            retObj.IsSuccessful = true;
            retObj.RateLimited = false;
            retObj.CastList = JsonConvert.DeserializeObject<List<TVMazeCastItem>>(response.Content);
            return retObj;
        }
        if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        {
            retObj.IsSuccessful = false;
            retObj.RateLimited = true;
            _logger.LogInformation($"response--TOOMANY. {id} getting tv maze cast by id :{id} - TOO MANY");
        }
        else
        {
            retObj.IsSuccessful = false;
            retObj.RateLimited = false;
            _logger.LogInformation($"response--ERROR. {id} getting tv maze cast by id :{id} - ERROR");
        }
        return retObj;
    }

    private async Task<RestResponse> TVMazeCastByShowIdHttpGetCall(int id)
    {
        try
        {
            var baseurl = "https://api.tvmaze.com";
            _logger.LogInformation($"getting tv maze cast by id :{id}");
            var options = new RestClientOptions(baseurl)
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            var client = new RestClient(options)
            {
            };
            var request = new RestRequest($"shows/{id}/cast");

            var response = await client.GetAsync(request);
            return response;
        }
        catch (WebException ex)
        {
            _logger.LogError($"{id} - web exception error {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{id} - generic web exception error {ex.Message}. Type {ex.GetType().Name}");
        }
        return null;
    }

    public async Task<TVMazeCastHttpCallResponse> ScrapeAsync(int index)
    {
        //_logger.LogDebug($"Mint repo action is being executed... Token Id is {nFTEntity.TokenId}");

        var result = await GetTVMazeCastById(index);

        if (result.IsSuccessful)
        {
            _logger.LogInformation($"get tv maze cast by id is successfull id is {index}");
            if(result.CastList == null || !result.CastList.Any()){
                 _logger.LogInformation($"{index} - cast list is empty");
                 return result;
            }
            var entity = new TVMazeRecordEntity()
            {
                Index = index,
                CastList = result.CastList,
                ProductionType = "Movie"
            };
             var dataModel = _mapper.Map<TVMazeRecordEntity, TVMazeRecordDataModel>(entity);
        await this.SaveAsync(new List<TVMazeRecordDataModel>(){
            dataModel
        });
        }
        else if (!result.IsSuccessful && result.RateLimited)
        {
            _logger.LogInformation($"get tv maze cast by id is rate limited. id is {index}");
        }
        else
        {
            _logger.LogInformation($"things have gone wrong dude. id is {index}");
        }
        return result;

       
    }


    public async Task AddScrapeTaskAsync(AddScrapeTaskCommand command)
    {
        _logger.LogDebug($"Burn repo action is being executed... Token Id is {command.StartIndex}");
        var publishRequest = new PublishRequest()
        {
            Message = JsonConvert.SerializeObject(command),
            TopicArn = TopicArn
        };
        await _snsClient.PublishAsync(publishRequest);
        _logger.LogDebug($"Burn repo action is successfully executed... Token Id is {command.StartIndex}");
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
        var results = await GetBySortKeyAsync(nameof(TVMazeRecordDataModel.TVMazeType), walletId);
        var mappedEntity = _mapper.Map<List<TVMazeRecordDataModel>, List<TVMazeRecordEntity>>(results);
        _logger.LogDebug($"Get tokens from wallet repo action is successfully executed for wallet with Id {walletId}. Returning result");
        return mappedEntity;
    }

    public async Task ResetAsync()
    {
        await Task.CompletedTask;
        throw new NotSupportedException("Reset Action is not supported at his point");
    }
   
}
