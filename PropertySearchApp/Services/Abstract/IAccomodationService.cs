using LanguageExt.Common;
using PropertySearchApp.Domain;

namespace PropertySearchApp.Services.Abstract;

public interface IAccommodationService
{
    Task<IEnumerable<AccommodationDomain>> GetAccommodationsAsync();
    Task<Result<AccommodationDomain>> GetAccommodationByIdAsync(string accommodationId);
    Task<Result<bool>> UpdateAccommodationAsync(AccommodationDomain accommodation);
    Task<Result<bool>> DeleteAccommodationAsync(string accommodationId);
}