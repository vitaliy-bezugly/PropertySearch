using LanguageExt.Common;
using PropertySearchApp.Common.Exceptions;
using PropertySearchApp.Domain.Abstract;

namespace PropertySearchApp.Domain;

public class AccommodationDomain : DomainBase
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public int Price { get; set; }
    public Guid UserId { get; set; }
    public string? PhotoUri { get; set; }
    public LocationDomain? Location { get; set; }
    public DateTime CreationTime { get; set; }

    public AccommodationDomain() : base()
    {
        Title = string.Empty;
        Description = string.Empty;
        Price = 0;
        UserId = Guid.Empty;
    }
    public AccommodationDomain(Guid id, string title, string? description, int price, string? photoUri, Guid userId, LocationDomain location) : base(id)
    {
        Title = title;
        Description = description;
        Price = price;
        PhotoUri = photoUri;
        UserId = userId;
        Location = location;
    }

    public Result<bool> Validate()
    {
        var titleValidation = ValidateTitle();
        if (titleValidation.IsFaulted)
            return titleValidation;
        
        var priceValidation = ValidatePrice();
        if (priceValidation.IsFaulted)
            return priceValidation;

        return new Result<bool>(true);
    }

    public Result<bool> ValidateTitle()
    {
        if (String.IsNullOrEmpty(Title))
            return new Result<bool>(new AccommodationValidationException($"{nameof(Title)} is not valid"));
        return new Result<bool>(true);
    }
    public Result<bool> ValidatePrice()
    {
        if (Price < 0)
            return new Result<bool>(new AccommodationValidationException($"{nameof(Price)} is not valid"));
        return new Result<bool>(true);
    }
}