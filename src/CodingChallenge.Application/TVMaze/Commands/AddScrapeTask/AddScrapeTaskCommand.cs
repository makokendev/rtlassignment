using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CodingChallenge.Application.Interfaces;
using CodingChallenge.Application.TVMaze.Base;
using CodingChallenge.Domain.Entities.NFT;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CodingChallenge.Application.TVMaze.Commands.Burn;
public record AddScrapeTaskCommand(int StartIndex, int EndIndex,int TryCount) : TVMazeScrapeCommandBase(), IRequest<AddScrapeTaskCommandResponse>;
public record AddScrapeTaskCommandResponse(int StartIndex,int EndIndex) : TVMazeScrapeCommandResponseBase();

// public class AddScrapeTaskClass
// {
//     public int StartIndex { get; set; }
//     public int EndIndex { get; set; }
//     public int TryCount { get; set; }
// }
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
        await _repo.AddScrapeTaskAsync(request);
        var response = new AddScrapeTaskCommandResponse(request.StartIndex,request.EndIndex);
        return response;
    }

}
