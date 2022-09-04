using CodingChallenge.Application.TVMaze.Base;
using FluentValidation;

namespace CodingChallenge.Application.TVMaze.Commands.Mint;

public class ScrapeCommandValidator : AbstractValidator<ScrapeCommand>
{
    public ScrapeCommandValidator()
    {
        // RuleFor(v => v.TokenId)
        //     .NotEmpty()
        //     .Custom((tokenId, context) =>
        // {
        //     if (!tokenId.IsHex())
        //     {
        //         context.AddFailure("Token Id must be Hexadecimal");
        //     }
        // });
        // RuleFor(v => v.Address)
        //     .NotEmpty()
        //     .Custom((walletId, context) =>
        // {
        //     if (!walletId.IsHex())
        //     {
        //         context.AddFailure("Address Id must be Hexadecimal");
        //     }
        // });

    }
}

