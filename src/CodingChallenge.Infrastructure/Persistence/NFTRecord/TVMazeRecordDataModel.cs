using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using CodingChallenge.Application.AutoMapper;
using CodingChallenge.Domain.Base;
using CodingChallenge.Domain.Entities.NFT;

namespace CodingChallenge.Infrastructure.Persistence.TVMazeRecord;



public class TVMazeRecordDataModelTVMazeRecordEntityResolver : IValueResolver<TVMazeRecordDataModel, TVMazeRecordEntity, NFTWallet>
{
    public NFTWallet Resolve(TVMazeRecordDataModel source, TVMazeRecordEntity destination, NFTWallet member, ResolutionContext context) => new NFTWallet(source.TokenId);
}
public class TVMazeRecordDataModel : AuditableEntity, IMapFrom<TVMazeRecordEntity>
{

    [DynamoDBRangeKey]
    public int TokenId { get; set; }
    [DynamoDBHashKey]
    public string WalletId { get; set; }
    [DynamoDBProperty]
    public string Type { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<TVMazeRecordEntity, TVMazeRecordDataModel>()
            .ForMember(d => d.TokenId, opt => opt.MapFrom(s => s.TokenId))
            .ForMember(d => d.WalletId, opt => opt.MapFrom(s => s.Wallet.WalletId));
        profile.CreateMap<TVMazeRecordDataModel, TVMazeRecordEntity>()
            .ForMember(d => d.TokenId, opt => opt.MapFrom(s => s.TokenId))
            .ForMember(d => d.Wallet, opt => opt.MapFrom<TVMazeRecordDataModelTVMazeRecordEntityResolver>());
    }
}


