using LanguageExt.Common;
using PropertySearchApp.Entities;
using PropertySearchApp.Repositories.Abstract;

namespace PropertySearchApp.Repositories;

public class AccommodationRepository : IAccommodationRepository
{
    public Task<Result<bool>> DeleteAsync(string accommodationId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AccommodationEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result<AccommodationEntity>> GetAsync(string accommodationId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> UpdateAsync(AccommodationEntity accommodation)
    {
        throw new NotImplementedException();
    }
}