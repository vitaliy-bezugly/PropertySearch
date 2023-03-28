using LanguageExt.Common;
using PropertySearchApp.Entities;

namespace PropertySearchApp.Repositories.Abstract;

public interface IAccommodationRepository
{
    Task<IEnumerable<AccommodationEntity>> GetWithLimitsAsync(int startAt, int countOfItems, CancellationToken cancellationToken);
    Task<IEnumerable<AccommodationEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<AccommodationEntity?> GetAsync(Guid accommodationId, CancellationToken cancellationToken);
    Task<bool> CreateAsync(AccommodationEntity accommodation, CancellationToken cancellationToken);
    Task<Result<bool>> UpdateAsync(AccommodationEntity accommodation, CancellationToken cancellationToken);
    Task<Result<bool>> DeleteAsync(Guid accommodationId, CancellationToken cancellationToken);
}