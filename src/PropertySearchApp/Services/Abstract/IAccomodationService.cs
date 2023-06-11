using PropertySearchApp.Common;
using PropertySearchApp.Domain;

namespace PropertySearchApp.Services.Abstract;

public interface IAccommodationService
{
    Task<IEnumerable<AccommodationDomain>> GetWithLimitsAsync(int startAt, int countOfItems, CancellationToken cancellationToken);
    Task<IEnumerable<AccommodationDomain>> GetAccommodationsAsync(CancellationToken cancellationToken);
    Task<AccommodationDomain?> GetAccommodationByIdAsync(Guid accommodationId, CancellationToken cancellationToken);
    Task<OperationResult> CreateAccommodationAsync(AccommodationDomain accommodation, CancellationToken cancellationToken);
    Task<OperationResult> UpdateAccommodationAsync(AccommodationDomain accommodation, CancellationToken cancellationToken);
    Task<OperationResult> DeleteAccommodationAsync(Guid userId, Guid accommodationId, CancellationToken cancellationToken);
}