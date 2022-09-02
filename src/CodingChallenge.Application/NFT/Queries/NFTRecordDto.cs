using AutoMapper;
using CodingChallenge.Application.AutoMapper;
using CodingChallenge.Domain.Entities.NFT;

namespace CodingChallenge.Application.NFT.Queries;
public class TVMazeRecordDto : IMapFrom<TVMazeRecordEntity>
{
    public string TokenId { get; set; }

    public string WalletId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<TVMazeRecordEntity, TVMazeRecordDto>()
            .ForMember(d => d.TokenId, opt => opt.MapFrom(s => s.Index))
            .ForMember(d => d.WalletId, opt => opt.MapFrom(s => s.CastList));
   
    }
}
