using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using CodingChallenge.Application.TVMaze.Base;
using CodingChallenge.Application.TVMaze.Commands.Burn;
using CodingChallenge.Application.TVMaze.Commands.Mint;
using CodingChallenge.Application.TVMaze.Queries;
using CodingChallenge.Application.TVMaze.Queries.Token;

namespace CodingChallenge.Application.IntegrationTests.NFT.Commands;

public class CQRSTestBase : TestBase
{
    public CQRSTestBase()
    {
        Environment.SetEnvironmentVariable("UseInMemoryDatabase", "true");
        Init();
    }

    public string GenerateBigIntegerHexadecimal()
    {
        return $"{ValidationExtensions.HexadecimalPrefix}{new BigInteger(new Random().NextInt64()).ToString("X40")}";
    }

    public async Task<ScrapeCommandResponse> SendScrapeCommandAsync(int index=0)
    {
        var command = new ScrapeCommand(index);
        return await Sender.Send(command);
    }
    public async Task<TVMazeRecordDto> GetItemByIndexCommandAsync(int index=0)
    {
        var command = new GetTVMazeItemByIndexQuery(index.ToString());
        return await Sender.Send<TVMazeRecordDto>(command);
    }
    public async Task<AddScrapeTaskCommandResponse> BurnScrapeCommandAsync(int startIndex,int endIndex)
    {
        var command = new AddScrapeTaskCommand(startIndex,endIndex,0);
        return await Sender.Send(command);
    }

}
