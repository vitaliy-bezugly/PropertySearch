using LanguageExt.Common;
using PropertySearchApp.Domain;

namespace PropertySearchApp.Services.Abstract;

public interface IAccommodationService
{
    Task<IEnumerable<AccommodationDomain>> GetAccommodationsAsync(CancellationToken cancellationToken);
    Task<AccommodationDomain?> GetAccommodationByIdAsync(Guid accommodationId, CancellationToken cancellationToken);
    Task<Result<bool>> CreateAccommodationAsync(AccommodationDomain accommodation, CancellationToken cancellationToken);
    Task<Result<bool>> UpdateAccommodationAsync(AccommodationDomain accommodation, CancellationToken cancellationToken);
    Task<Result<bool>> DeleteAccommodationAsync(Guid userId, Guid accommodationId, CancellationToken cancellationToken);
}