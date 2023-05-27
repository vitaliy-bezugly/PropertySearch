using AutoMapper;
using FluentValidation;
using PropertySearchApp.Common;
using PropertySearchApp.Common.Constants;
using PropertySearchApp.Domain;
using PropertySearchApp.Entities;
using PropertySearchApp.Repositories.Abstract;
using PropertySearchApp.Services.Abstract;
using PropertySearchApp.Common.Extensions;
using PropertySearchApp.Common.Logging;

namespace PropertySearchApp.Services;

public class AccommodationService : IAccommodationService
{
    private readonly IAccommodationRepository _accommodationRepository;
    private readonly IUserValidatorService _userValidator;
    private readonly IMapper _mapper;
    private readonly IValidator<AccommodationDomain> _accommodationValidator;
    private readonly IValidator<LocationDomain> _locationValidator;
    private readonly ILogger<AccommodationService> _logger;
    public AccommodationService(IAccommodationRepository accommodationRepository, IMapper mapper, IUserValidatorService userValidator, IValidator<AccommodationDomain> accommodationValidator, IValidator<LocationDomain> locationValidator, ILogger<AccommodationService> logger)
    {
        _accommodationRepository = accommodationRepository;
        _mapper = mapper;
        _userValidator = userValidator;
        _accommodationValidator = accommodationValidator;
        _locationValidator = locationValidator;
        _logger = logger;
    }

    public async Task<IEnumerable<AccommodationDomain>> GetWithLimitsAsync(int startAt, int countOfItems, CancellationToken cancellationToken)
    {
        try
        {
            return (await _accommodationRepository.GetWithLimitsAsync(startAt, countOfItems, cancellationToken)).Select(x => _mapper.Map<AccommodationDomain>(x));
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(AccommodationService))
                .WithMethod(nameof(GetWithLimitsAsync))
                .WithOperation("Get")
                .WithComment(e.Message)
                .WithParameter(typeof(int).Name, nameof(startAt), startAt.ToString())
                .WithParameter(typeof(int).Name, nameof(countOfItems), countOfItems.ToString())
                .ToString());

            throw;
        }
    }
    
    public async Task<IEnumerable<AccommodationDomain>> GetAccommodationsAsync(CancellationToken cancellationToken)
    {
        try
        {
            return (await _accommodationRepository.GetAllAsync(cancellationToken)).Select(x => _mapper.Map<AccommodationDomain>(x));
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(AccommodationService))
                .WithMethod(nameof(GetAccommodationsAsync))
                .WithOperation("Get")
                .WithComment(e.Message)
                .WithNoParameters()
                .ToString());
            
            throw;
        }
    }
    
    public async Task<AccommodationDomain?> GetAccommodationByIdAsync(Guid accommodationId, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _accommodationRepository.GetAsync(accommodationId, cancellationToken);
            return entity == null ? null : _mapper.Map<AccommodationDomain>(entity);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(AccommodationService))
                .WithMethod(nameof(GetAccommodationByIdAsync))
                .WithOperation("Get")
                .WithComment(e.Message)
                .WithParameter(typeof(Guid).Name, nameof(accommodationId), accommodationId.ToString())
                .ToString());
            
            throw;
        }
    }
    
    public async Task<OperationResult> CreateAccommodationAsync(AccommodationDomain accommodation, CancellationToken cancellationToken)
    {
        try
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
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(AccommodationService))
                .WithMethod(nameof(CreateAccommodationAsync))
                .WithOperation("Post")
                .WithComment(e.Message)
                .WithParameter(typeof(AccommodationDomain).FullName, nameof(accommodation), accommodation.SerializeObject())
                .ToString());
            
            throw;
        }
    }
    public async Task<OperationResult> UpdateAccommodationAsync(AccommodationDomain accommodation, CancellationToken cancellationToken)
    {
        try
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
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(AccommodationService))
                .WithMethod(nameof(UpdateAccommodationAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(typeof(AccommodationDomain).FullName, nameof(accommodation), accommodation.SerializeObject())
                .ToString());
            
            throw;
        }
    }
    public async Task<OperationResult> DeleteAccommodationAsync(Guid userId, Guid accommodationId, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _userValidator.ValidateAsync(userId, accommodationId, true);
            if (validationResult.Succeeded == false)
            {
                return validationResult;
            }
        
            var deletionResult = await _accommodationRepository.DeleteAsync(accommodationId, cancellationToken);
            return deletionResult;
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(AccommodationService))
                .WithMethod(nameof(DeleteAccommodationAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(typeof(Guid).Name, nameof(userId), userId.ToString())
                .WithParameter(typeof(Guid).Name, nameof(accommodationId), accommodationId.ToString())
                .ToString());
            
            throw;
        }
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