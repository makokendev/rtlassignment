namespace CodingChallenge.Application.NFT.Base;
public abstract record TVMazeScrapeCommandResponseBase
{
    public TVMazeCommandType TransactionType { get; }
    public TVMazeScrapeCommandResponseBase(TVMazeCommandType transactionType)
    {
        TransactionType = transactionType;
    }
    public bool IsSuccess
    {
        get
        {
            return string.IsNullOrWhiteSpace(ErrorMessage);
        }
    }
    public string ErrorMessage { get; internal set; }
}