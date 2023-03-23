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
        
        var (validationResult, validationError) = await ValidateUserFieldsAsync(accommodation.UserId);
        if (validationResult == false)
        {
            return validationError;
        }
        
        var creationResult = await _accommodationRepository.CreateAsync(_mapper.Map<AccommodationEntity>(accommodation), cancellationToken);
        return new Result<bool>(creationResult);
    }

    public async Task<Result<bool>> UpdateAccommodationAsync(AccommodationDomain accommodation, CancellationToken cancellationToken)
    {
        var (validationResult, validationError) = await ValidateUserFieldsAndAccessAsync(accommodation.UserId, accommodation.Id);
        if (validationResult == false)
        {
            return validationError;
        }
        
        var updateResult = await _accommodationRepository.UpdateAsync(_mapper.Map<AccommodationEntity>(accommodation), cancellationToken);
        return updateResult;
    }

    public async Task<Result<bool>> DeleteAccommodationAsync(Guid userId, Guid accommodationId, CancellationToken cancellationToken)
    {
        var (validationResult, validationError) = await ValidateUserFieldsAndAccessAsync(userId, accommodationId);
        if (validationResult == false)
        {
            return validationError;
        }
        
        var deletionResult = await _accommodationRepository.DeleteAsync(accommodationId, cancellationToken);
        return deletionResult;
    }

    private async Task<(bool, Result<bool>)> ValidateUserFieldsAsync(Guid userId)
    {
        var validationResult = await _userValidator.ValidateAsync(userId);
        if (validationResult.IsFaulted)
        {
            return (false, validationResult);
        }
        
        return (true, new Result<bool>(true));
    }
    private async Task<(bool, Result<bool>)> ValidateUserFieldsAndAccessAsync(Guid userId, Guid accommodationId)
    {
        var (validationResult, validationError) = await ValidateUserFieldsAsync(userId);
        if (validationResult == false)
        {
            return (false, validationError);
        }
        
        var validationAccessResult = await _userValidator.ValidateAccessToAccommodationAsync(userId, accommodationId);
        if (validationAccessResult.IsFaulted)
        {
            return (false, validationAccessResult);
        }

        return (true, new Result<bool>(true));
    }
}