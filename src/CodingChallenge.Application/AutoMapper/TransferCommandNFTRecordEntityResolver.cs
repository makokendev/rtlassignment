using AutoMapper;
using CodingChallenge.Application.NFT.Commands.Transfer;
using CodingChallenge.Domain.Entities.NFT;

namespace CodingChallenge.Application.AutoMapper;

public class TransferCommandTVMazeRecordEntityResolver : IValueResolver<TransferCommand, TVMazeRecordEntity, NFTWallet>
{
    public NFTWallet Resolve(TransferCommand source, TVMazeRecordEntity destination, NFTWallet member, ResolutionContext context) => new NFTWallet(source.From);
}
