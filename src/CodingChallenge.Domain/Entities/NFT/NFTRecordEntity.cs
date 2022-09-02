using System.Collections.Generic;
using CodingChallenge.Domain.Base;

namespace CodingChallenge.Domain.Entities.NFT;
public class TVMazeRecordEntity : AuditableEntity
{
    public int Index { get; set; }
    public string ProductionType { get; set; }
    public List<TVMazeCastItem> CastList { get; set; }
}
