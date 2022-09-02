using CodingChallenge.Application.NFT.Base;
using CodingChallenge.Application.NFT.Commands.Burn;
using CodingChallenge.Application.NFT.Commands.Mint;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CodingChallenge.EventQueueProcessor;
public class TVMazeScrapeCommandController
{
    private readonly ISender _mediator;

    private readonly ILogger _logger;

    public TVMazeScrapeCommandController(ILogger logger, ISender sender)
    {
        _logger = logger;
        _mediator = sender;
    }

    public async Task<List<TVMazeScrapeCommandResponseBase>> ProcessCommandListAsync(List<TVMazeScrapeCommandBase> commandList)
    {
        var responseList = new List<TVMazeScrapeCommandResponseBase>();
        foreach (var command in commandList)
        {
            responseList.Add(await ExecuteTransactionCommandBaseAsync(command));
        }
        return responseList;
    }

    public async Task<TVMazeScrapeCommandResponseBase> ExecuteTransactionCommandBaseAsync(TVMazeScrapeCommandBase command)
    {
        if (command is ScrapeCommand)
        {
            _logger.LogDebug($"command with token id is ScrapeCommand");
            return await ScrapeAsync(command as ScrapeCommand);
        }
        return null;
    }

    public async Task<ScrapeCommandResponse> ScrapeAsync(ScrapeCommand ScrapeCommand)
    {
        _logger.LogDebug($"scrape  command is called for token id {ScrapeCommand.index}");
        return await _mediator.Send<ScrapeCommandResponse>(ScrapeCommand);
    }
     public async Task<AddScrapeTaskCommandResponse> AddScrapeTaskAsync(AddScrapeTaskCommand addScrapeTaskCommand)
    {
        return await _mediator.Send<AddScrapeTaskCommandResponse>(addScrapeTaskCommand);
    }
}
