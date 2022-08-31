
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CodingChallenge.Application.Interfaces;
using CodingChallenge.Domain.Entities.NFT;
using MediatR;

namespace CodingChallenge.Application.NFT.Queries.Wallet;
public record GetNFTsFromWalletQuery(string WalletId) : IRequest<List<TVMazeRecordDto>>;

public class GetNFTsFromWalletQueryHandler : IRequestHandler<GetNFTsFromWalletQuery, List<TVMazeRecordDto>>
{
    private readonly ITVMazeRecordRepository _repository;
    private readonly IMapper _mapper;

    public GetNFTsFromWalletQueryHandler(ITVMazeRecordRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<TVMazeRecordDto>> Handle(GetNFTsFromWalletQuery request, CancellationToken cancellationToken)
    {
        var responseEntity = await _repository.GetByWalletIdAsync(request.WalletId);
        return _mapper.Map<List<TVMazeRecordEntity>, List<TVMazeRecordDto>>(responseEntity);
    }
}
