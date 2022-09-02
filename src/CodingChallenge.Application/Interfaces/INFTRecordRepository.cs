using System.Collections.Generic;
using System.Threading.Tasks;
using CodingChallenge.Application.NFT.Commands.Burn;
using CodingChallenge.Domain.Entities;
using CodingChallenge.Domain.Entities.NFT;

namespace CodingChallenge.Application.Interfaces;
public interface ITVMazeRecordRepository
{
    Task<TVMazeCastHttpCallResponse> GetTVMazeCastById(int id);
    Task<List<TVMazeRecordEntity>> GetByWalletIdAsync(string walletId);
    Task<TVMazeRecordEntity> GetByTokenIdAsync(string tokenId);
    Task AddScrapeTaskAsync(AddScrapeTaskCommand command);
    Task<TVMazeCastHttpCallResponse> ScrapeAsync(int index);
   
}
