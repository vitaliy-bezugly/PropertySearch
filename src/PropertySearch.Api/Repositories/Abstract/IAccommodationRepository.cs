using PropertySearch.Api.Common;
using PropertySearch.Api.Entities;

namespace PropertySearch.Api.Repositories.Abstract;

public interface IAccommodationRepository : IRepository<AccommodationEntity>
{
    Task<IEnumerable<AccommodationEntity>> GetWithLimitsAsync(int startAt, int countOfItems, CancellationToken cancellationToken);
    Task<IEnumerable<AccommodationEntity>> GetUserAccommodationsWithLimitsAsync(Guid userId, int startAt, int countOfItems, CancellationToken cancellationToken);
    Task<int> GetCountAsync(CancellationToken cancellationToken);
    Task<int> GetUserAccommodationsCountAsync(Guid userId, CancellationToken cancellationToken);
}