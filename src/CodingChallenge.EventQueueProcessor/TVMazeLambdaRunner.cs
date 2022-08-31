using CodingChallenge.Application.NFT.Base;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace CodingChallenge.EventQueueProcessor;

public class TVMazeLambdaRunner
{
    ILogger _logger;
    TVMazeScrapeCommandController _TVMazeRecordCommandHandler;
    public TVMazeLambdaRunner(ILogger logger, TVMazeScrapeCommandController handler)
    {
        _logger = logger;
        _TVMazeRecordCommandHandler = handler;
    }


    public async Task HandleInlineJsonOptionAsync(string inlineJson)
    {
        _logger.LogDebug($"HandleGetWalletDataOption.HandleInlineJsonOption is being processed... {inlineJson}");
        var token = JToken.Parse(inlineJson);
        List<TVMazeScrapeCommandBase> listOfCommands = new List<TVMazeScrapeCommandBase>();
        if (token is JArray)
        {
            listOfCommands.AddRange(inlineJson.ParseListOfTransactionCommands(_logger));
        }
        else if (token is JObject)
        {
            listOfCommands.Add(inlineJson.GetTransactionCommandFromJsonString(_logger));
        }
        if (listOfCommands.Any())
        {
            _logger.LogInformation($"There are {listOfCommands.Count} transaction(s). Processing command list");
            var listOfCommandResponses = await _TVMazeRecordCommandHandler.ProcessCommandListAsync(listOfCommands);
            _logger.LogInformation($"Read {listOfCommands.Count} transaction(s)");
            foreach (var response in listOfCommandResponses)
            {
                if (!response.IsSuccess)
                {
                    _logger.LogError($"Error occurred for {response.IsSuccess}. Error message: {response.ErrorMessage}");
                }
            }
        }
        else
        {
            _logger.LogInformation($"{inlineJson} is not valid json");
        }

    }

}