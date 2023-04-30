using AutoMapper;
using FluentValidation;
using PropertySearchApp.Common;
using PropertySearchApp.Common.Constants;
using PropertySearchApp.Domain;
using PropertySearchApp.Entities;
using PropertySearchApp.Repositories.Abstract;
using PropertySearchApp.Services.Abstract;
using System.Threading;

namespace PropertySearchApp.Services;

public class AccommodationService : IAccommodationService
{
    private readonly IAccommodationRepository _accommodationRepository;
    private readonly IUserValidatorService _userValidator;
    private readonly IMapper _mapper;
    private readonly IValidator<AccommodationDomain> _accommodationValidator;
    private readonly IValidator<LocationDomain> _locationValidator;
    public AccommodationService(IAccommodationRepository accommodationRepository, IMapper mapper, IUserValidatorService userValidator, IValidator<AccommodationDomain> accommodationValidator, IValidator<LocationDomain> locationValidator)
    {
        _accommodationRepository = accommodationRepository;
        _mapper = mapper;
        _userValidator = userValidator;
        _accommodationValidator = accommodationValidator;
        _locationValidator = locationValidator;
    }

    public async Task<IEnumerable<AccommodationDomain>> GetWithLimitsAsync(int startAt, int countOfItems, CancellationToken cancellationToken)
    {
        return (await _accommodationRepository.GetWithLimitsAsync(startAt, countOfItems, cancellationToken)).Select(x => _mapper.Map<AccommodationDomain>(x));
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
    public async Task<OperationResult> CreateAccommodationAsync(AccommodationDomain accommodation, CancellationToken cancellationToken)
    {
        var accommodationValidationResult = await ValidateAccommodationAsync(accommodation, cancellationToken);
        if (accommodationValidationResult.Succeeded == false)
            return accommodationValidationResult;

        var locationValidationResult = await ValidateLocationAsync(accommodation.Location, cancellationToken);
        if (locationValidationResult.Succeeded == false)
            return locationValidationResult;

        var userValidationResult = await _userValidator.ValidateAsync(accommodation.UserId, accommodation.Id, false);
        if (userValidationResult.Succeeded == false)
        {
            return userValidationResult;
        }
        
        var creationResult = await _accommodationRepository.CreateAsync(_mapper.Map<AccommodationEntity>(accommodation), cancellationToken);
        return creationResult == true ? new OperationResult()
            : new OperationResult(ErrorMessages.UnhandledInternalError);
    }
    public async Task<OperationResult> UpdateAccommodationAsync(AccommodationDomain accommodation, CancellationToken cancellationToken)
    {
        var accommodationValidationResult = await ValidateAccommodationAsync(accommodation, cancellationToken);
        if (accommodationValidationResult.Succeeded == false)
            return accommodationValidationResult;

        var validationResult = await _userValidator.ValidateAsync(accommodation.UserId, accommodation.Id, true);
        if (validationResult.Succeeded == false)
        {
            return validationResult;
        }
        
        var updateResult = await _accommodationRepository.UpdateAsync(_mapper.Map<AccommodationEntity>(accommodation), cancellationToken);
        return updateResult;
    }
    public async Task<OperationResult> DeleteAccommodationAsync(Guid userId, Guid accommodationId, CancellationToken cancellationToken)
    {
        var validationResult = await _userValidator.ValidateAsync(userId, accommodationId, true);
        if (validationResult.Succeeded == false)
        {
            return validationResult;
        }
        
        var deletionResult = await _accommodationRepository.DeleteAsync(accommodationId, cancellationToken);
        return deletionResult;
    }

    private async Task<OperationResult> ValidateAccommodationAsync(AccommodationDomain accommodation, CancellationToken cancellationToken)
    {
        var validationResult = await _accommodationValidator.ValidateAsync(accommodation, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return new OperationResult(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        return new OperationResult();
    }
    private async Task<OperationResult> ValidateLocationAsync(LocationDomain location, CancellationToken cancellationToken)
    {
        var validationResult = await _locationValidator.ValidateAsync(location, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return new OperationResult(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        return new OperationResult();
    }
}