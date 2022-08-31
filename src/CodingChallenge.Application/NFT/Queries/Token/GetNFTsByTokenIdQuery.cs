using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CodingChallenge.Application.Interfaces;
using CodingChallenge.Domain.Entities.NFT;
using MediatR;

namespace CodingChallenge.Application.NFT.Queries.Token;

public record GetNFTsByTokenIdQuery(string TokenId) : IRequest<TVMazeRecordDto>;

public class GetNFTsByTokenIdQueryHandler : IRequestHandler<GetNFTsByTokenIdQuery, TVMazeRecordDto>
{
    private readonly ITVMazeRecordRepository repo;
    private readonly IMapper _mapper;

    public GetNFTsByTokenIdQueryHandler(ITVMazeRecordRepository context, IMapper mapper)
    {
        repo = context;
        _mapper = mapper;
    }

    public async Task<TVMazeRecordDto> Handle(GetNFTsByTokenIdQuery request, CancellationToken cancellationToken)
    {
        var responseEntity = await repo.GetByTokenIdAsync(request.TokenId);
        return _mapper.Map<TVMazeRecordEntity, TVMazeRecordDto>(responseEntity);
    }
}
