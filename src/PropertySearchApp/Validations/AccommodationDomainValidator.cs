using FluentValidation;
using PropertySearchApp.Common.Constants;
using PropertySearchApp.Domain;

namespace PropertySearchApp.Validations;

public class AccommodationDomainValidator : AbstractValidator<AccommodationDomain>
{
    public AccommodationDomainValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(ErrorMessages.Accommodation.Validation.EmptyTitle);

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage(ErrorMessages.Accommodation.Validation.NegativePrice);

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(ErrorMessages.Accommodation.Validation.EmptyUserId);

        RuleFor(x => x.Location)
            .NotEmpty()
            .WithMessage(ErrorMessages.Accommodation.Validation.NullLocation);
    }
}
