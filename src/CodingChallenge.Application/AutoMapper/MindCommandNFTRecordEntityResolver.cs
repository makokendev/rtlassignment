using AutoMapper;
using CodingChallenge.Application.NFT.Commands.Mint;
using CodingChallenge.Domain.Entities.NFT;

namespace CodingChallenge.Application.AutoMapper;

public class MindCommandTVMazeRecordEntityResolver : IValueResolver<ScrapeCommand, TVMazeRecordEntity, NFTWallet>
{
    public NFTWallet Resolve(ScrapeCommand source, TVMazeRecordEntity destination, NFTWallet member, ResolutionContext context) => new NFTWallet(source.Address);
}
