using CodingChallenge.Application.TVMaze.Base;
using CodingChallenge.Application.TVMaze.Commands.Burn;
using CodingChallenge.Application.TVMaze.Commands.Mint;
using CodingChallenge.Application.TVMaze.Queries;
using CodingChallenge.Application.TVMaze.Queries.Token;
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
        return await _TVMazeRecordCommandHandler.ScrapeAsync(new Application.TVMaze.Commands.Mint.ScrapeCommand(index));

    }
    public async Task<AddScrapeTaskCommandResponse> AddScrapeTaskAsync(AddScrapeTaskCommand addScrapeTaskCommand)
    {
        return await _TVMazeRecordCommandHandler.AddScrapeTaskAsync(addScrapeTaskCommand);
    }
    public async Task<TVMazeRecordDto> GetTokenByIdAsync(GetTVMazeItemByIndexQuery query)
    {
        return await _TVMazeRecordCommandHandler.GetTokenByIdAsync(query);
    }
    

}