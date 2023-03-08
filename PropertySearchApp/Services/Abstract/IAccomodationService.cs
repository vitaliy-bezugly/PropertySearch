using LanguageExt.Common;
using PropertySearchApp.Domain;

namespace PropertySearchApp.Services.Abstract;

public interface IAccommodationService
{
    Task<IEnumerable<AccommodationDomain>> GetAccommodationsAsync();
    Task<Result<AccommodationDomain>> GetAccommodationByIdAsync(Guid accommodationId);
    Task<Result<bool>> UpdateAccommodationAsync(Guid userId, AccommodationDomain accommodation);
    Task<Result<bool>> DeleteAccommodationAsync(Guid userId, Guid accommodationId);
}