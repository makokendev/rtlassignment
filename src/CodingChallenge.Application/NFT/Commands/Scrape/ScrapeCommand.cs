using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CodingChallenge.Application.Exceptions;
using CodingChallenge.Application.Interfaces;
using CodingChallenge.Domain.Entities.NFT;
using MediatR;
using Microsoft.Extensions.Logging;
using CodingChallenge.Application.NFT.Base;

namespace CodingChallenge.Application.NFT.Commands.Mint;

public record ScrapeCommand(string TokenId,string Address) : TVMazeScrapeCommandBase(TokenId,TVMazeCommandType.Scrap), IRequest<ScrapeCommandResponse>;

public record ScrapeCommandResponse(string TokenId,string WalletId) : TVMazeScrapeCommandResponseBase(TVMazeCommandType.Scrap);

public class ScrapeCommandHandler : IRequestHandler<ScrapeCommand, ScrapeCommandResponse>
{
    public ScrapeCommandHandler(ITVMazeRecordRepository repo, ILogger logger, IMapper mapper)
    {
        _repo = repo;
        _logger = logger;
        _mapper = mapper;
    }

    public ITVMazeRecordRepository _repo { get; }
    public ILogger _logger { get; }

    public IMapper _mapper { get; }

    public async Task<ScrapeCommandResponse> Handle(ScrapeCommand request, CancellationToken cancellationToken)
    {
        var retRec = new ScrapeCommandResponse(request.TokenId, request.Address);
        try
        {
            var entity = _mapper.Map<ScrapeCommand, TVMazeRecordEntity>(request);
            entity.Created = DateTime.Now;
            entity.CreatedBy = "CurrentUserId";
            _logger.LogDebug($"handling.... ScrapeCommandHandler... id:{entity.TokenId} -- sortkey:{entity.Wallet}");
            await _repo.ScrapeAsync(entity);

        }
        catch (NFTTokenAlreadyExistsException ex)
        {
            retRec.ErrorMessage = ex.Message;
        }
        return retRec;
    }
}
