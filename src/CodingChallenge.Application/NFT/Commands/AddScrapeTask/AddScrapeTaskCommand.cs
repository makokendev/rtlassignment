using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CodingChallenge.Application.Interfaces;
using CodingChallenge.Application.NFT.Base;
using CodingChallenge.Domain.Entities.NFT;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CodingChallenge.Application.NFT.Commands.Burn;
public record AddScrapeTaskCommand(string TokenId) : TVMazeScrapeCommandBase(TokenId,TVMazeCommandType.Burn), IRequest<AddScrapeTaskCommandResponse>;
public record AddScrapeTaskCommandResponse(string TokenId) : TVMazeScrapeCommandResponseBase(TVMazeCommandType.Burn);

public class AddScrapeTaskCommandHandler : IRequestHandler<AddScrapeTaskCommand, AddScrapeTaskCommandResponse>
{
    public AddScrapeTaskCommandHandler(ITVMazeRecordRepository repo, ILogger logger, IMapper mapper)
    {
        _repo = repo;
        _logger = logger;
        _mapper = mapper;
    }

    public ITVMazeRecordRepository _repo { get; }
    public ILogger _logger { get; }

    public IMapper _mapper { get; }

    public async Task<AddScrapeTaskCommandResponse> Handle(AddScrapeTaskCommand request, CancellationToken cancellationToken)
    {
        var entity = new TVMazeRecordEntity()
        {
            TokenId = request.TokenId,
        };

        await _repo.AddScrapeTaskAsync(request.TokenId);
        _logger.LogDebug($"Dispatching event... response id is {entity.TokenId}...");
        var response = new AddScrapeTaskCommandResponse(request.TokenId);
        _logger.LogDebug($"deleted the entry hopefully response id is {response.TokenId}");
        return response;
    }

}
