using FluentValidation;
using CodingChallenge.Application.NFT.Base;
namespace CodingChallenge.Application.NFT.Commands.Burn;

public class AddScrapeTaskCommandValidator : AbstractValidator<AddScrapeTaskCommand>
{
    public AddScrapeTaskCommandValidator()
    {
        RuleFor(v => v.TokenId)
           .NotEmpty()
           .Custom((tokenId, context) =>
       {
           if (!tokenId.IsHex())
           {
               context.AddFailure("Token Id must be Hexadecimal");
           }
       });
    }
}

