using AutoMapper;
using LanguageExt.Common;
using PropertySearchApp.Domain;
using PropertySearchApp.Entities;
using PropertySearchApp.Repositories.Abstract;
using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Services;

public class AccommodationService : IAccommodationService
{
    private readonly IAccommodationRepository _accommodationRepository;
    private readonly IUserValidatorService _userValidator;
    private readonly IMapper _mapper;
    public AccommodationService(IAccommodationRepository accommodationRepository, IMapper mapper, IUserValidatorService userValidator)
    {
        _accommodationRepository = accommodationRepository;
        _mapper = mapper;
        _userValidator = userValidator;
    }

    public async Task<IEnumerable<AccommodationDomain>> GetAccommodationsAsync(CancellationToken cancellationToken)
    {
        return (await _accommodationRepository.GetAllAsync(cancellationToken)).Select(x => _mapper.Map<AccommodationDomain>(x));
    }

    public async Task<AccommodationDomain?> GetAccommodationByIdAsync(Guid accommodationId, CancellationToken cancellationToken)
    {
        var entity = await _accommodationRepository.GetAsync(accommodationId, cancellationToken);
        return entity == null ? null : _mapper.Map<AccommodationDomain>(entity);
    }

    public async Task<Result<bool>> CreateAccommodationAsync(AccommodationDomain accommodation, CancellationToken cancellationToken)
    {
        var accommodationValidationResult = accommodation.Validate();
        if (accommodationValidationResult.IsFaulted)
            return accommodationValidationResult;
        
        var validationResult = await _userValidator.ValidateAsync(accommodation.UserId, accommodation.Id, false);
        if (validationResult.IsFaulted)
        {
            return validationResult;
        }
        
        var creationResult = await _accommodationRepository.CreateAsync(_mapper.Map<AccommodationEntity>(accommodation), cancellationToken);
        return new Result<bool>(creationResult);
    }

    public async Task<Result<bool>> UpdateAccommodationAsync(AccommodationDomain accommodation, CancellationToken cancellationToken)
    {
        var accommodationValidationResult = accommodation.Validate();
        if (accommodationValidationResult.IsFaulted)
            return accommodationValidationResult;
        
        var validationResult = await _userValidator.ValidateAsync(accommodation.UserId, accommodation.Id, true);
        if (validationResult.IsFaulted)
        {
            return validationResult;
        }
        
        var updateResult = await _accommodationRepository.UpdateAsync(_mapper.Map<AccommodationEntity>(accommodation), cancellationToken);
        return updateResult;
    }

    public async Task<Result<bool>> DeleteAccommodationAsync(Guid userId, Guid accommodationId, CancellationToken cancellationToken)
    {
        var validationResult = await _userValidator.ValidateAsync(userId, accommodationId, true);
        if (validationResult.IsFaulted)
        {
            return validationResult;
        }
        
        var deletionResult = await _accommodationRepository.DeleteAsync(accommodationId, cancellationToken);
        return deletionResult;
    }
}