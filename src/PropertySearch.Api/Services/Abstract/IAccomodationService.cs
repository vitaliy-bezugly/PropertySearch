using PropertySearch.Api.Common;
using PropertySearch.Api.Domain;

namespace PropertySearch.Api.Services.Abstract;

public interface IAccommodationService
{
    Task<PaginatedList<AccommodationDomain>> GetPaginatedCollection(PaginationQueryDomain query, CancellationToken cancellationToken);
    Task<PaginatedList<AccommodationDomain>> GetUserAccommodationsAsync(Guid userId, PaginationQueryDomain query, CancellationToken cancellationToken);
    Task<AccommodationDomain?> GetAccommodationByIdAsync(Guid accommodationId, CancellationToken cancellationToken);
    Task<OperationResult> CreateAccommodationAsync(AccommodationDomain accommodation, CancellationToken cancellationToken);
    Task<OperationResult> UpdateAccommodationAsync(AccommodationDomain accommodation, CancellationToken cancellationToken);
    Task<OperationResult> DeleteAccommodationAsync(Guid userId, Guid accommodationId, CancellationToken cancellationToken);
}