using Microsoft.EntityFrameworkCore;
using PropertySearchApp.Common;
using PropertySearchApp.Common.Constants;
using PropertySearchApp.Common.Exceptions;
using PropertySearchApp.Entities;
using PropertySearchApp.Persistence;
using PropertySearchApp.Repositories.Abstract;

namespace PropertySearchApp.Repositories;

public class AccommodationRepository : IAccommodationRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AccommodationRepository> _logger;
    public AccommodationRepository(ApplicationDbContext context, ILogger<AccommodationRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<IEnumerable<AccommodationEntity>> GetWithLimitsAsync(int startAt, int countOfItems, CancellationToken cancellationToken)
    {
        return await _context.Accommodations
            .Include(x => x.Location)
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Skip(startAt)
            .Take(countOfItems)
            .ToListAsync(cancellationToken);
    }
    public async Task<IEnumerable<AccommodationEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Accommodations.Include(x => x.Location).AsNoTracking().ToListAsync(cancellationToken);
    }
    public async Task<AccommodationEntity?> GetAsync(Guid accommodationId, CancellationToken cancellationToken)
    {
        return await _context.Accommodations.Include(x => x.Location).AsNoTracking().FirstOrDefaultAsync(x => x.Id == accommodationId, cancellationToken);
    }
    public async Task<bool> CreateAsync(AccommodationEntity accommodation, CancellationToken cancellationToken)
    {
        ValidateAccommodationFieldsIfInvalidThrowException(accommodation);
        
        await _context.Accommodations.AddAsync(accommodation, cancellationToken);
        var result = await _context.SaveChangesAsync(cancellationToken);
        if (result > 0) /* Can not add accommodation */
        {
            return true;
        }
        
        _logger.LogWarning($"Can not add accommodation with title: {accommodation.Title}", accommodation);
        return false;
    }
    public async Task<OperationResult> UpdateAsync(AccommodationEntity destination, CancellationToken cancellationToken)
    {
        ValidateAccommodationFieldsIfInvalidThrowException(destination);

        var source = await _context.Accommodations.Include(x => x.Location).FirstOrDefaultAsync(x => x.Id == destination.Id, cancellationToken);
        if (source == null)
        {
            _logger.LogWarning("Can not update accommodation");
            return new OperationResult(ErrorMessages.Accommodation.NotFound);
        }

        UpdateFields(source, destination);

        var result = await _context.SaveChangesAsync(cancellationToken);
        return ValidateNumberOfWrittenDatabaseEntriesAndReturnResultState(result,
            ErrorMessages.Accommodation.CanNotUpdate);
    }
    public async Task<OperationResult> DeleteAsync(Guid accommodationId, CancellationToken cancellationToken)
    {
        var exists = await _context.Accommodations.FirstOrDefaultAsync(x => x.Id == accommodationId, cancellationToken);
        if (exists == null)
        {
            _logger.LogWarning( "Can not delete accommodation");
            return new OperationResult(ErrorMessages.Accommodation.NotFound);
        }

        _context.Accommodations.Remove(exists);
        var result = await _context.SaveChangesAsync(cancellationToken);
        return ValidateNumberOfWrittenDatabaseEntriesAndReturnResultState(result, ErrorMessages.Accommodation.CanNotDelete);
    }

    private static void UpdateFields(AccommodationEntity source, AccommodationEntity destination)
    {
        source.Title = destination.Title;
        source.Description = destination.Description;
        source.Price = destination.Price;
        source.PhotoUri = destination.PhotoUri;

        if (destination.Location is not null)
        {
            source.Location.Country = destination.Location.Country;
            source.Location.City = destination.Location.City;
            source.Location.Region = destination.Location.Region;
            source.Location.Address = destination.Location.Address;
        }
    }
    private void ValidateAccommodationFieldsIfInvalidThrowException(AccommodationEntity accommodation)
    {
        if (accommodation == null || accommodation.UserId == Guid.Empty)
            throw new ArgumentNullException($"{nameof(accommodation)} and {nameof(accommodation.UserId)} can not be null or empty");
    }

    private OperationResult ValidateNumberOfWrittenDatabaseEntriesAndReturnResultState(int entriesNumber, string errorMessage)
    {
        if (entriesNumber > 0)
        {
            return new OperationResult();
        }

        var error = new InternalDatabaseException(new[] { errorMessage });
        _logger.LogWarning(error, "Internal database error. Can not complete operation");
        return new OperationResult(errorMessage);
    }
}