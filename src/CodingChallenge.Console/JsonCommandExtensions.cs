using System;
using System.Collections.Generic;
using System.IO;
using CodingChallenge.Application.TVMaze.Base;
using CodingChallenge.Application.TVMaze.Commands.Burn;
using CodingChallenge.Application.TVMaze.Commands.Mint;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CodingChallenge.Console;
public static class JsonCommandExtensions
{
    public static TVMazeScrapeCommandBase GetTransactionCommandFromJsonString(this string itemJsonText, ILogger logger)
    {
        dynamic dynamicObject = JsonConvert.DeserializeObject(itemJsonText);
        var transactionType = dynamicObject?.Type?.ToString();
        logger.LogDebug($"Transaction type is {transactionType}");
        if (string.IsNullOrEmpty(transactionType))
        {
            var ex = new Exception("Type property is missing from the command object");
            logger.LogError(ex, ex.Message);
        }
        var tryParseResult = Enum.TryParse(transactionType, true, out TVMazeCommandType transactionTypeResult);
        switch (transactionTypeResult)
        {
            case TVMazeCommandType.Scrap:
                {
                    return JsonConvert.DeserializeObject<ScrapeCommand>(itemJsonText);
                }
            case TVMazeCommandType.Burn:
                {
                    return JsonConvert.DeserializeObject<AddScrapeTaskCommand>(itemJsonText);
                }
          
            default: break;

        }
        return null;
    }
    public static List<TVMazeScrapeCommandBase> ParseJsonFile(this string filePath, ILogger logger)
    {
        if (!File.Exists(filePath))
        {
            var ex = new FileNotFoundException($"{filePath} json file is not found. Please check the arguments", filePath);
            logger.LogError(ex, ex.Message);
            throw ex;
        }
        var jsonText = File.ReadAllText(filePath);
        logger.LogDebug($"json text is {jsonText}");
        return jsonText.ParseListOfTransactionCommands(logger);
    }

    public static List<TVMazeScrapeCommandBase> ParseListOfTransactionCommands(this string jsonText, ILogger logger)
    {
        dynamic deserializedList = JsonConvert.DeserializeObject(jsonText);
        var transactionList = ParseListOfCommands(deserializedList, logger);
        return transactionList;
    }

    public static List<TVMazeScrapeCommandBase> ParseListOfCommands(dynamic deserializedList, ILogger logger)
    {
        logger.LogDebug($"deserialised list is being parsed...");
        var transactionList = new List<TVMazeScrapeCommandBase>();
        foreach (var listItem in deserializedList)
        {
            var commandJson = listItem?.ToString() as string;
            var getItem = commandJson.GetTransactionCommandFromJsonString(logger);
            transactionList.Add(getItem);
        }
        return transactionList;
    }
}
