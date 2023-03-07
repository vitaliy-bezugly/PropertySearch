using LanguageExt.Common;
using PropertySearchApp.Entities;

namespace PropertySearchApp.Repositories.Abstract;

public interface IAccommodationRepository
{
    Task<IEnumerable<AccommodationEntity>> GetAllAsync();
    Task<Result<AccommodationEntity>> GetAsync(string accommodationId);
    Task<Result<bool>> UpdateAsync(AccommodationEntity accommodation);
    Task<Result<bool>> DeleteAsync(string accommodationId);
}