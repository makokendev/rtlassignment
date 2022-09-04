using System.Collections.Generic;
using AutoMapper;
using CodingChallenge.Application.AutoMapper;
using CodingChallenge.Domain.Entities;
using CodingChallenge.Domain.Entities.NFT;

namespace CodingChallenge.Application.TVMaze.Queries;
public class TVMazeRecordDto : IMapFrom<TVMazeRecordEntity>
{
    public int Index { get; set; }
    public string ProductionType { get; set; }
    public List<TVMazeCastItem> CastList { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<TVMazeRecordEntity, TVMazeRecordDto>();

    }
}
