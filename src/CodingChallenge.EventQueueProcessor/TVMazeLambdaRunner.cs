using CodingChallenge.Application.NFT.Base;
using CodingChallenge.Application.NFT.Commands.Burn;
using CodingChallenge.Application.NFT.Commands.Mint;
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


    public async Task<ScrapeCommandResponse> SendScrapeCommand(int index)
    {
        return await _TVMazeRecordCommandHandler.ScrapeAsync(new Application.NFT.Commands.Mint.ScrapeCommand(index));

    }
    public async Task<AddScrapeTaskCommandResponse> AddScrapeTaskAsync(AddScrapeTaskCommand addScrapeTaskCommand)
    {
         return await _TVMazeRecordCommandHandler.AddScrapeTaskAsync(addScrapeTaskCommand);
    }

}