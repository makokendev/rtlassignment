// using System.Threading.Tasks;
// using CodingChallenge.Application.Exceptions;
// using CodingChallenge.Application.NFT.Commands.Mint;
// using Xunit;

// namespace CodingChallenge.Application.IntegrationTests.NFT.Commands;

// public class ScrapeCommandTests : CQRSTestBase
// {
//     [Fact]
//     public async Task ShouldRequireMinimumFields()
//     {
//         var command = new ScrapeCommand(null,null);
//         var exception = await Assert.ThrowsAsync<RequestValidationException>(async () => await Sender.Send(command));
//         Assert.True(exception.Errors.Count > 0);
//         Assert.NotNull(exception.Errors[nameof(command.TokenId)]);
//     }

//     [Fact]
//     public async Task TokenidShouldBeValidHexadecimal()
//     {
//         var command = new ScrapeCommand("non-hex",null);
//         //assert
//         var exception = await Assert.ThrowsAsync<RequestValidationException>(async () => await Sender.Send(command));
//         Assert.True(exception.Errors.Count > 0);
//         Assert.NotNull(exception.Errors[nameof(command.TokenId)]);
//     }
//     [Fact]
//     public async Task WalletIdIsRequired()
//     {
//         var tokenId = GenerateBigIntegerHexadecimal();
//         var command = new ScrapeCommand(tokenId,null);
//         //assert
//         RequestValidationException exception = await Assert.ThrowsAsync<RequestValidationException>(async () => await Sender.Send(command));
//         Assert.True(exception.Errors.Count > 0);
//         Assert.NotNull(exception.Errors[nameof(command.Address)]);
//     }

//     [Fact]
//     public async Task MintTransactionShouldSucceed()
//     {
//         var tokenId = GenerateBigIntegerHexadecimal();
//         var walletId = GenerateBigIntegerHexadecimal();
//         var command = new ScrapeCommand(tokenId,walletId);

//         var response = await Sender.Send(command);
//         Assert.Equal(response.TokenId, tokenId);
//         Assert.Equal(response.WalletId, walletId);

//     }
// }
