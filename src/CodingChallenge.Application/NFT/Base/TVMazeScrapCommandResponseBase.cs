namespace CodingChallenge.Application.NFT.Base;
public abstract record TVMazeScrapeCommandResponseBase
{
    public TVMazeScrapeCommandResponseBase()
    {
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