using PropertySearch.Api.Common;
using PropertySearch.Api.Entities;

namespace PropertySearch.Api.Repositories.Abstract;

public interface IAccommodationRepository
{
    Task<IEnumerable<AccommodationEntity>> GetWithLimitsAsync(int startAt, int countOfItems, CancellationToken cancellationToken);
    Task<IEnumerable<AccommodationEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<AccommodationEntity>> GetUserAccommodationsWithLimitsAsync(Guid userId, int startAt, int countOfItems, CancellationToken cancellationToken);
    Task<int> GetCountAsync(CancellationToken cancellationToken);
    Task<int> GetUserAccommodationsCountAsync(Guid userId, CancellationToken cancellationToken);
    Task<AccommodationEntity?> GetAsync(Guid accommodationId, CancellationToken cancellationToken);
    Task<bool> CreateAsync(AccommodationEntity accommodation, CancellationToken cancellationToken);
    Task<OperationResult> UpdateAsync(AccommodationEntity accommodation, CancellationToken cancellationToken);
    Task<OperationResult> DeleteAsync(Guid accommodationId, CancellationToken cancellationToken);
}