using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CodingChallenge.Application.Interfaces;
using CodingChallenge.Application.NFT.Base;
using CodingChallenge.Domain.Entities.NFT;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CodingChallenge.Application.NFT.Commands.Burn;
public record AddScrapeTaskCommand(string StartIndex,string EndIndex) : TVMazeScrapeCommandBase(), IRequest<AddScrapeTaskCommandResponse>;
public record AddScrapeTaskCommandResponse(string TokenId) : TVMazeScrapeCommandResponseBase();

public class AddScrapeTaskCommandHandler : IRequestHandler<AddScrapeTaskCommand, AddScrapeTaskCommandResponse>
{
    public AddScrapeTaskCommandHandler(ITVMazeRecordRepository repo, ILogger logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public ITVMazeRecordRepository _repo { get; }
    public ILogger _logger { get; }


    public async Task<AddScrapeTaskCommandResponse> Handle(AddScrapeTaskCommand request, CancellationToken cancellationToken)
    {
        var entity = new TVMazeRecordEntity()
        {
        };

        await _repo.AddScrapeTaskAsync(request.StartIndex,request.EndIndex);
        _logger.LogDebug($"Dispatching event... response id is {entity.TokenId}...");
        var response = new AddScrapeTaskCommandResponse(request.StartIndex);
        _logger.LogDebug($"deleted the entry hopefully response id is {response.TokenId}");
        return response;
    }

}
