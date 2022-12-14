using AutoMapper;
using CodingChallenge.Application.TVMaze.Commands.Mint;
using CodingChallenge.Domain.Entities.NFT;

namespace CodingChallenge.Application.AutoMapper;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        typeof(MappingProfile).Assembly.ApplyMappingsFromAssembly(this);
        // MapScrapeCommand();
        // MapTransferCommand();
    }

    // private void MapScrapeCommand()
    // {
    //     CreateMap<ScrapeCommand, TVMazeRecordEntity>()
    //         .ForMember(dest => dest.TokenId, a => a.MapFrom(o => o.index))
    //         .ForMember(dest => dest.Wallet, a => a.MapFrom<MindCommandTVMazeRecordEntityResolver>());
    // }
    // private void MapTransferCommand()
    // {
    //     CreateMap<TransferCommand, TVMazeRecordEntity>()
    //         .ForMember(dest => dest.TokenId, a => a.MapFrom(o => o.TokenId))
    //         .ForMember(dest => dest.Wallet, a => a.MapFrom<TransferCommandTVMazeRecordEntityResolver>());
    // }


}
