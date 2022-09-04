using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CodingChallenge.Application.Interfaces;
using CodingChallenge.Domain.Entities.NFT;
using MediatR;

namespace CodingChallenge.Application.TVMaze.Queries.Token;

public record GetTVMazeItemByIndexQuery(string Index) : IRequest<TVMazeRecordDto>;

public class GetTVMazeItemByIndexQueryHandler : IRequestHandler<GetTVMazeItemByIndexQuery, TVMazeRecordDto>
{
    private readonly ITVMazeRecordRepository repo;
    private readonly IMapper _mapper;

    public GetTVMazeItemByIndexQueryHandler(ITVMazeRecordRepository context, IMapper mapper)
    {
        repo = context;
        _mapper = mapper;
    }

    public async Task<TVMazeRecordDto> Handle(GetTVMazeItemByIndexQuery request, CancellationToken cancellationToken)
    {
        var responseEntity = await repo.GetByIndexAsync(request.Index);
        return _mapper.Map<TVMazeRecordEntity, TVMazeRecordDto>(responseEntity);
    }
}
