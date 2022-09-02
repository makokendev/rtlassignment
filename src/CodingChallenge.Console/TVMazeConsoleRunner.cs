using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodingChallenge.Application.Exceptions;
using CodingChallenge.Application.NFT.Commands.Mint;
using CommandLine;
using Microsoft.Extensions.Logging;

namespace CodingChallenge.Console;

public class TVMazeConsoleRunner
{
    ILogger _logger;
    TVMazeScrapeCommandController _TVMazeRecordCommandHandler;
    public TVMazeConsoleRunner(ILogger logger, TVMazeScrapeCommandController handler)
    {
        _logger = logger;
        _TVMazeRecordCommandHandler = handler;
    }

    public async Task RunOptionsAsync(CommandLineOptions opts)
    {
        try
        {
            await HandleOptionsAsync();
        }
        catch (RequestValidationException rve)
        {
            _logger.LogError($"Validation error!");
            if (rve.Errors.Any())
            {
                foreach (var error in rve.Errors)
                {
                    _logger.LogError($"error: {error.Key} - {string.Join("-", error.Value)}");
                }
            }

        }
        catch (Exception ex)
        {
            _logger.LogError($"CodingChallenge Request: Unhandled Exception. Message: {ex.Message}");
        }
    }

    private async Task HandleOptionsAsync()
    {
        _logger.LogDebug($"file is being passed...");
        var lastId = 0;
        for (int i = 1; i <= 200; i++)
        {
            if (i % 10 == 0)
            {
                _logger.LogInformation($"modules ok last id {lastId}, index {i}");
                var response = await _TVMazeRecordCommandHandler.AddScrapeTaskAsync(new Application.NFT.Commands.Burn.AddScrapeTaskCommand((lastId+1),i,0));
                lastId = i;
            }
        }
    }

    public async Task HandleParseErrorAsync(IEnumerable<Error> errs)
    {

        //help requested and version requested are built in and can be ignored.
        if (errs.Any(e => e.Tag != ErrorType.HelpRequestedError && e.Tag != ErrorType.VersionRequestedError))
        {
            foreach (var error in errs)
            {
                _logger.LogWarning($"Command line parameter parse error. {error.ToString()}");
            }
        }
        await Task.CompletedTask;
    }
}