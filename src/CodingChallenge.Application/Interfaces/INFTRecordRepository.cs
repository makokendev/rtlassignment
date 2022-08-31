using System.Collections.Generic;
using System.Threading.Tasks;
using CodingChallenge.Domain.Entities.NFT;

namespace CodingChallenge.Application.Interfaces;
public interface ITVMazeRecordRepository
{
    Task<List<TVMazeRecordEntity>> GetByWalletIdAsync(string walletId);
    Task<TVMazeRecordEntity> GetByTokenIdAsync(string tokenId);
    Task AddScrapeTaskAsync(string id);
    Task ScrapeAsync(TVMazeRecordEntity nFTEntity);
    Task TransferAsync(TVMazeRecordEntity nFTEntity, string newWalletId);
    Task ResetAsync();
}
