using LanguageExt.Common;
using PropertySearchApp.Entities;
using PropertySearchApp.Repositories.Abstract;

namespace PropertySearchApp.Repositories;

public class AccommodationRepository : IAccommodationRepository
{
    public Task<Result<bool>> DeleteAsync(Guid userId, Guid accommodationId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AccommodationEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result<AccommodationEntity>> GetAsync(Guid accommodationId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> UpdateAsync(Guid userId, AccommodationEntity accommodation)
    {
        throw new NotImplementedException();
    }
}