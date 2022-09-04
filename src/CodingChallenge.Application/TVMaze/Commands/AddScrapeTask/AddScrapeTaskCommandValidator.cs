using FluentValidation;
using CodingChallenge.Application.TVMaze.Base;
namespace CodingChallenge.Application.TVMaze.Commands.Burn;

public class AddScrapeTaskCommandValidator : AbstractValidator<AddScrapeTaskCommand>
{
    public AddScrapeTaskCommandValidator()
    {
    //     RuleFor(v => v.TokenId)
    //        .NotEmpty()
    //        .Custom((tokenId, context) =>
    //    {
    //     //    if (!tokenId.IsHex())
    //     //    {
    //     //       // context.AddFailure("Token Id must be Hexadecimal");
    //     //    }
    //    });
    }
}

