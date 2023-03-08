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

    public Task<Result<bool>> DeleteAccommodationAsync(Guid userId, Guid accommodationId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AccommodationDomain>> GetAccommodationByIdAsync(Guid accommodationId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AccommodationDomain>> GetAccommodationsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> UpdateAccommodationAsync(Guid userId, AccommodationDomain accommodation)
    {
        throw new NotImplementedException();
    }
}