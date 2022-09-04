using FluentValidation;
using System;

namespace CodingChallenge.Application.TVMaze.Queries.Token;

public class GetTVMazeItemByIndexQueryValidator : AbstractValidator<GetTVMazeItemByIndexQuery>
{
    public GetTVMazeItemByIndexQueryValidator()
    {
        RuleFor(v => v.Index)
            .NotEmpty()
            .Custom((index, context) =>
        {
            if (!Int32.TryParse(index,out _))
            {
                context.AddFailure("Index must be an integer");
            }
        });
    }
}