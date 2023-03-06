using LanguageExt.Common;
using PropertySearchApp.Domain;
using PropertySearchApp.Repositories.Abstract;
using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Services;

public class AccommodationService : IAccommodationService
{
    private readonly IAccommodationRepository _repository;

    public AccommodationService(IAccommodationRepository repository)
    {
        _repository = repository;
    }

    public Result<IEnumerable<AccommodationDomain>> GetAccommodations()
    {
        throw new NotImplementedException();
    }

    public Result<AccommodationDomain?> GetAccommodationById(string accommodationId)
    {
        throw new NotImplementedException();
    }

    public Result<bool> UpdateAccommodation(AccommodationDomain accommodation)
    {
        throw new NotImplementedException();
    }

    public Result<bool> DeleteAccommodation(string accommodationId)
    {
        throw new NotImplementedException();
    }
}