using LanguageExt.Common;
using PropertySearchApp.Entities;

namespace PropertySearchApp.Repositories.Abstract;

public interface IAccommodationRepository
{
    Result<IEnumerable<AccommodationEntity>> GetAll();
    Result<AccommodationEntity?> Get(string accommodationId);
    Result<bool> Update(AccommodationEntity accommodation);
    Result<bool> Delete(string accommodationId);
}