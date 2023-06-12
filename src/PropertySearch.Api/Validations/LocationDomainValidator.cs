using FluentValidation;
using PropertySearch.Api.Common.Constants;
using PropertySearch.Api.Domain;

namespace PropertySearch.Api.Validations;

public class LocationDomainValidator : AbstractValidator<LocationDomain>
{
    public LocationDomainValidator()
    {
        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage(ErrorMessages.Location.Validation.EmptyCountry);
        RuleFor(x => x.Region)
            .NotEmpty()
            .WithMessage(ErrorMessages.Location.Validation.EmptyRegion);
        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage(ErrorMessages.Location.Validation.EmptyCity);
        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage(ErrorMessages.Location.Validation.EmptyAddress)
            .MinimumLength(5)
            .WithMessage(ErrorMessages.Location.Validation.AddressMinLength);
    }
}
