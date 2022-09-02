// using System;
// using System.Collections.Generic;
// using System.Numerics;
// using System.Threading.Tasks;
// using CodingChallenge.Application.NFT.Base;
// using CodingChallenge.Application.NFT.Commands.Burn;
// using CodingChallenge.Application.NFT.Commands.Mint;
// using CodingChallenge.Application.NFT.Queries;
// using CodingChallenge.Application.NFT.Commands.Reset;
// using CodingChallenge.Application.NFT.Commands.Transfer;
// using CodingChallenge.Application.NFT.Queries.Token;
// using CodingChallenge.Application.NFT.Queries.Wallet;

// namespace CodingChallenge.Application.IntegrationTests.NFT.Commands;

// public class CQRSTestBase : TestBase
// {
//     public CQRSTestBase()
//     {
//         Environment.SetEnvironmentVariable("UseInMemoryDatabase", "true");
//         Init();
//     }

//     public string GenerateBigIntegerHexadecimal()
//     {
//         return $"{ValidationExtensions.HexadecimalPrefix}{new BigInteger(new Random().NextInt64()).ToString("X40")}";
//     }

//     public async Task<ScrapeCommandResponse> SendScrapeCommandAsync(string tokenId=null, string walletId=null)
//     {
//         if (string.IsNullOrEmpty(tokenId))
//             tokenId = GenerateBigIntegerHexadecimal();
//         if (string.IsNullOrEmpty(walletId))
//             walletId = GenerateBigIntegerHexadecimal();
//         var command = new ScrapeCommand(tokenId);

//         return await Sender.Send(command);
//     }
//     public async Task<AddScrapeTaskCommandResponse> BurnScrapeCommandAsync(string tokenId)
//     {
//         var command = new AddScrapeTaskCommand(tokenId,"asdasd");
//         return await Sender.Send(command);
//     }
//     public async Task<ResetCommandResponse> ResetCommandAsync()
//     {
//         var command = new ResetCommand();
//         return await Sender.Send(command);
//     }
//     public async Task<TransferCommandResponse> TransferScrapeCommandAsync(string tokenId, string fromAddress, string toAddress)
//     {
//         var command = new TransferCommand(tokenId,fromAddress,toAddress);

//         return await Sender.Send(command);
//     }
//     public async Task<TVMazeRecordDto> GetNFTByIdQueryAsync(string tokenId)
//     {
//         return await Sender.Send<TVMazeRecordDto>(new GetNFTsByTokenIdQuery(tokenId));
//     }
//     public async Task<List<TVMazeRecordDto>> GetNFTByWalletIdQueryAsync(string walletId)
//     {
//         return await Sender.Send<List<TVMazeRecordDto>>(new GetNFTsFromWalletQuery(walletId));
//     }

// }
