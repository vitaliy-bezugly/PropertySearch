using AutoMapper;
using FluentValidation;
using PropertySearch.Api.Common;
using PropertySearch.Api.Common.Constants;
using PropertySearch.Api.Common.Logging;
using PropertySearch.Api.Domain;
using PropertySearch.Api.Entities;
using PropertySearch.Api.Repositories.Abstract;
using PropertySearch.Api.Services.Abstract;
using PropertySearch.Api.Common.Extensions;

namespace PropertySearch.Api.Services;

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

    public async Task<PaginatedList<AccommodationDomain>> GetPaginatedCollection(PaginationQueryDomain query, CancellationToken cancellationToken)
    {
        try
        {
            int startAt = (query.PageNumber - 1) * query.PageSize;
            IEnumerable<AccommodationEntity> accommodations = await _accommodationRepository
                .GetWithLimitsAsync(startAt, query.PageSize, cancellationToken);
            
            int totalCount = await _accommodationRepository.GetCountAsync(cancellationToken);
            
            var paginatedList = new PaginatedList<AccommodationDomain>(
                accommodations.Select(x => _mapper.Map<AccommodationDomain>(x)).ToList(),
                query.PageNumber,
                (int)Math.Ceiling(totalCount / (double)query.PageSize),
                totalCount
            );
            
            return paginatedList;
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(AccommodationService))
                .WithMethod(nameof(GetPaginatedCollection))
                .WithOperation("Get")
                .WithComment(e.Message)
                .WithParameter(nameof(PaginationQueryDomain), nameof(query), query.SerializeObject())
                .ToString());

            throw;
        }
    }

    public async Task<PaginatedList<AccommodationDomain>> GetUserAccommodationsAsync(Guid userId, PaginationQueryDomain query, CancellationToken cancellationToken)
    {
        try
        {
            int startAt = (query.PageNumber - 1) * query.PageSize;
            IEnumerable<AccommodationEntity> accommodations = await _accommodationRepository
                .GetUserAccommodationsWithLimitsAsync(userId, startAt, query.PageSize, cancellationToken);
            
            var accommodationsDomain = accommodations
                .Select(x => _mapper.Map<AccommodationDomain>(x))
                .ToList();

            int totalCount = await _accommodationRepository.GetUserAccommodationsCountAsync(userId, cancellationToken);

            var paginatedList = new PaginatedList<AccommodationDomain>(
                accommodationsDomain,
                query.PageNumber,
                (int)Math.Ceiling(totalCount / (double)query.PageSize),
                totalCount);
            
            return paginatedList;
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(AccommodationService))
                .WithMethod(nameof(GetUserAccommodationsAsync))
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
                .WithParameter(nameof(Guid), nameof(accommodationId), accommodationId.ToString())
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
            return creationResult ? new OperationResult()
                : new OperationResult(ErrorMessages.UnhandledInternalError);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(AccommodationService))
                .WithMethod(nameof(CreateAccommodationAsync))
                .WithOperation("Post")
                .WithComment(e.Message)
                .WithParameter(typeof(AccommodationDomain).FullName ?? String.Empty, nameof(accommodation), accommodation.SerializeObject())
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
                .WithParameter(typeof(AccommodationDomain).FullName ?? String.Empty, nameof(accommodation), accommodation.SerializeObject())
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
    
    private async Task<OperationResult> ValidateLocationAsync(LocationDomain? location, CancellationToken cancellationToken)
    {
        var validationResult = await _locationValidator.ValidateAsync(location!, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return new OperationResult(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        return new OperationResult();
    }
}