using Chirp.Core.Entities;
using FluentValidation;

namespace Chirp.Infrastructure.Repository;

public class CheepCreateValidator : AbstractValidator<CreateCheep>
{
    public CheepCreateValidator()
    {
        // @TODO Check that these values are correct: 
        RuleFor(x => x.Text).NotEmpty().MaximumLength(160).MinimumLength(5).WithMessage("The Cheep must be between 5 and 160 characters.(CheepCreateValidator)");
    }
}