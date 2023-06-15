using AutoMapper;
using FluentValidation;
using PropertySearch.Api.Common;
using PropertySearch.Api.Common.Logging;
using PropertySearch.Api.Domain;
using PropertySearch.Api.Entities;
using PropertySearch.Api.Services.Abstract;
using PropertySearch.Api.Common.Extensions;
using PropertySearch.Api.Persistence;

namespace PropertySearch.Api.Services;

public class AccommodationService : IAccommodationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserValidatorService _userValidator;
    private readonly IMapper _mapper;
    private readonly IValidator<AccommodationDomain> _accommodationValidator;
    private readonly IValidator<LocationDomain> _locationValidator;
    private readonly ILogger<AccommodationService> _logger;
    public AccommodationService(IUnitOfWork unitOfWork, IMapper mapper, IUserValidatorService userValidator, IValidator<AccommodationDomain> accommodationValidator, IValidator<LocationDomain> locationValidator, ILogger<AccommodationService> logger)
    {
        _unitOfWork = unitOfWork;
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
            IEnumerable<AccommodationEntity> accommodations = await _unitOfWork.AccommodationRepository
                .GetWithLimitsAsync(startAt, query.PageSize, cancellationToken);
            
            int totalCount = await _unitOfWork.AccommodationRepository.GetCountAsync(cancellationToken);
            
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
            IEnumerable<AccommodationEntity> accommodations = await _unitOfWork.AccommodationRepository
                .GetUserAccommodationsWithLimitsAsync(userId, startAt, query.PageSize, cancellationToken);
            
            var accommodationsDomain = accommodations
                .Select(x => _mapper.Map<AccommodationDomain>(x))
                .ToList();

            int totalCount = await _unitOfWork.AccommodationRepository.GetUserAccommodationsCountAsync(userId, cancellationToken);

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
            var entity = await _unitOfWork.AccommodationRepository.GetByIdAsync(accommodationId, cancellationToken);
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
        
            _unitOfWork.AccommodationRepository.Insert(_mapper.Map<AccommodationEntity>(accommodation));
            await _unitOfWork.CommitAsync(cancellationToken);

            return OperationResult.Success;
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
        
            await _unitOfWork.AccommodationRepository.UpdateAsync(_mapper.Map<AccommodationEntity>(accommodation), cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            
            return OperationResult.Success;
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
        
            await _unitOfWork.AccommodationRepository.DeleteAsync(accommodationId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Success;
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(AccommodationService))
                .WithMethod(nameof(DeleteAccommodationAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(nameof(Guid), nameof(userId), userId.ToString())
                .WithParameter(nameof(Guid), nameof(accommodationId), accommodationId.ToString())
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

        return OperationResult.Success;
    }
    
    private async Task<OperationResult> ValidateLocationAsync(LocationDomain? location, CancellationToken cancellationToken)
    {
        var validationResult = await _locationValidator.ValidateAsync(location!, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return new OperationResult(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        return OperationResult.Success;
    }
}