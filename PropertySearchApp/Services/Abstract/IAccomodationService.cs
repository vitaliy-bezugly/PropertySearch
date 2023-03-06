using LanguageExt.Common;
using PropertySearchApp.Domain;

namespace PropertySearchApp.Services.Abstract;

public interface IAccommodationService
{
    Result<IEnumerable<AccommodationDomain>> GetAccommodations();
    Result<AccommodationDomain?> GetAccommodationById(string accommodationId);
    Result<bool> UpdateAccommodation(AccommodationDomain accommodation);
    Result<bool> DeleteAccommodation(string accommodationId);
}