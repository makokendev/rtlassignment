using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodingChallenge.Application.Exceptions;
using CodingChallenge.Application.TVMaze.Commands.Burn;
using CodingChallenge.Application.TVMaze.Commands.Mint;
using CommandLine;
using Microsoft.Extensions.Logging;

namespace CodingChallenge.Console;

public class TVMazeConsoleRunner
{
    public const int TotalNumberOfRecords = 65000;
    public const int ItemPerMessage = 10;
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
            await SendInScrapeOrder();
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

    private async Task SendInScrapeOrder()
    {
        _logger.LogDebug($"file is being passed...");
        var lastId = 0;
        var tasks = new List<Task>();
        _logger.LogInformation($"starting to end tasks ");
        for (int i = 1; i <= TotalNumberOfRecords; i++)
        {
            if (i % ItemPerMessage == 0)
            {
                _logger.LogInformation($"{i} - modules ok last id {lastId}, index ");
                tasks.Add(_TVMazeRecordCommandHandler.AddScrapeTaskAsync(new Application.TVMaze.Commands.Burn.AddScrapeTaskCommand((lastId + 1), i, 0)));
                lastId = i;
            }
        }
        _logger.LogInformation($"all the orders are given ");
        await Task.WhenAll(tasks);
        var results = new List<AddScrapeTaskCommandResponse>();
         _logger.LogInformation($"going over results");
        foreach (var task in tasks)
        {
            var result = ((Task<AddScrapeTaskCommandResponse>)task).Result;
            _logger.LogInformation($"got the result... {result.StartIndex} - {result.EndIndex}");
        }
        _logger.LogInformation($"should be all done.");
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