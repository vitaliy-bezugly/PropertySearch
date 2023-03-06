using LanguageExt.Common;
using PropertySearchApp.Entities;
using PropertySearchApp.Repositories.Abstract;

namespace PropertySearchApp.Repositories;

public class AccommodationRepository : IAccommodationRepository
{
    public Result<IEnumerable<AccommodationEntity>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Result<AccommodationEntity?> Get(string accommodationId)
    {
        throw new NotImplementedException();
    }

    public Result<bool> Update(AccommodationEntity accommodation)
    {
        throw new NotImplementedException();
    }

    public Result<bool> Delete(string accommodationId)
    {
        throw new NotImplementedException();
    }
}