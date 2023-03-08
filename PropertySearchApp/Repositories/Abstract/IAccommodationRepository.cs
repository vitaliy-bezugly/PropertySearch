using LanguageExt.Common;
using PropertySearchApp.Entities;

namespace PropertySearchApp.Repositories.Abstract;

public interface IAccommodationRepository
{
    Task<IEnumerable<AccommodationEntity>> GetAllAsync();
    Task<Result<AccommodationEntity>> GetAsync(Guid accommodationId);
    Task<Result<bool>> UpdateAsync(Guid userId, AccommodationEntity accommodation);
    Task<Result<bool>> DeleteAsync(Guid userId, Guid accommodationId);
}