using CodingChallenge.Application.NFT.Base;
using CodingChallenge.Application.NFT.Commands.Burn;
using CodingChallenge.Application.NFT.Commands.Mint;
using CodingChallenge.Application.NFT.Queries;
using CodingChallenge.Application.NFT.Commands.Reset;
using CodingChallenge.Application.NFT.Commands.Transfer;
using MediatR;
using Microsoft.Extensions.Logging;
using CodingChallenge.Application.NFT.Queries.Wallet;
using CodingChallenge.Application.NFT.Queries.Token;

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
            _logger.LogDebug($"command with token id {command.TokenId} is ScrapeCommand");
            return await ScrapeAsync(command as ScrapeCommand);
        }
        return null;
    }

    public async Task<ScrapeCommandResponse> ScrapeAsync(ScrapeCommand ScrapeCommand)
    {
        _logger.LogDebug($"mint command is called for token id {ScrapeCommand.TokenId}");
        return await _mediator.Send<ScrapeCommandResponse>(ScrapeCommand);
    }
    public async Task<AddScrapeTaskCommandResponse> BurnAsync(AddScrapeTaskCommand AddScrapeTaskCommand)
    {
        _logger.LogDebug($"burn command is called for token id {AddScrapeTaskCommand.TokenId}");
        return await _mediator.Send<AddScrapeTaskCommandResponse>(AddScrapeTaskCommand);
    }
    public async Task<ResetCommandResponse> ResetAsync(ResetCommand resetCommand)
    {
        _logger.LogDebug($"Reset command is called for token id {resetCommand}");
        return await _mediator.Send<ResetCommandResponse>(resetCommand);
    }
    public async Task<TransferCommandResponse> TransferAsync(TransferCommand transferCommand)
    {
        _logger.LogDebug($"transfer command is called for token id {transferCommand.TokenId}");
        return await _mediator.Send<TransferCommandResponse>(transferCommand);
    }
    public async Task<List<TVMazeRecordDto>> GetWalletContentAsync(GetNFTsFromWalletQuery query)
    {
        _logger.LogDebug($"wallet content query is called for token id {query.WalletId}");
        return await _mediator.Send<List<TVMazeRecordDto>>(query);
    }
    public async Task<TVMazeRecordDto> GetTokenByIdAsync(GetNFTsByTokenIdQuery query)
    {
        _logger.LogDebug($"token query is called for token id {query.TokenId}");
        return await _mediator.Send<TVMazeRecordDto>(query);
    }
}
