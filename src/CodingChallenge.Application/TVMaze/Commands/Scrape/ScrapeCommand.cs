using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CodingChallenge.Application.Exceptions;
using CodingChallenge.Application.Interfaces;
using CodingChallenge.Domain.Entities.NFT;
using MediatR;
using Microsoft.Extensions.Logging;
using CodingChallenge.Application.TVMaze.Base;
using System.Linq;

namespace CodingChallenge.Application.TVMaze.Commands.Mint;

public record ScrapeCommand(int index) : TVMazeScrapeCommandBase(), IRequest<ScrapeCommandResponse>;

public record ScrapeCommandResponse(int index) : TVMazeScrapeCommandResponseBase()
{
    public bool CastListEmpty { get; set; }
    public bool NotFound { get; set; }
    public bool RateLimited { get; internal set; }
}

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
        var retRec = new ScrapeCommandResponse(request.index);
        try
        {
            var result = await _repo.ScrapeAsync(request.index);
            if(result.CastList == null || !result.CastList.Any()){
                retRec.CastListEmpty = true;
            }
            if (!result.IsSuccessful)
            {
                retRec.ErrorMessage = $"not successful.";
                retRec.NotFound = result.NotFound;
            }
            if (result.RateLimited)
            {
                retRec.RateLimited = result.RateLimited;
                retRec.ErrorMessage = $"rate limited.";
            }
        }
        catch (TVMazeItemAlreadyExistsException ex)
        {
            retRec.ErrorMessage = ex.Message;
        }
        return retRec;
    }
}
