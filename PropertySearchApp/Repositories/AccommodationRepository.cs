using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
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
    public async Task<IEnumerable<AccommodationEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Accommodations.ToListAsync(cancellationToken);
    }
    public async Task<AccommodationEntity?> GetAsync(Guid accommodationId, CancellationToken cancellationToken)
    {
        return await _context.Accommodations.FirstOrDefaultAsync(x => x.Id == accommodationId, cancellationToken);
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

    public async Task<Result<bool>> UpdateAsync(AccommodationEntity accommodation, CancellationToken cancellationToken)
    {
        ValidateAccommodationFieldsIfInvalidThrowException(accommodation);

        var exists = await _context.Accommodations.FirstOrDefaultAsync(x => x.Id == accommodation.Id, cancellationToken);
        if (exists == null)
        {
            var exception = new AccommodationDataSourceException(new string[] {"There is no accommodation with given parameters"});
            _logger.LogWarning(exception, "Can not update accommodation");
            return new Result<bool>(exception);
        }

        exists.Title = accommodation.Title;
        exists.Description = accommodation.Description;
        exists.Price = accommodation.Price;
        
        var result = await _context.SaveChangesAsync(cancellationToken);
        return ValidateNumberOfWrittenDatabaseEntriesAndReturnResultState(result,
            $"Can not update accommodation with title: {accommodation.Title}");
    }

    public async Task<Result<bool>> DeleteAsync(Guid accommodationId, CancellationToken cancellationToken)
    {
        var exists = await _context.Accommodations.FirstOrDefaultAsync(x => x.Id == accommodationId, cancellationToken);
        if (exists == null)
        {
            var exception = new AccommodationDataSourceException(new string[] {"There is no accommodation with given id"});
            _logger.LogWarning(exception, "Can not delete accommodation");
            return new Result<bool>(exception);
        }

        _context.Accommodations.Remove(exists);
        var result = await _context.SaveChangesAsync(cancellationToken);
        return ValidateNumberOfWrittenDatabaseEntriesAndReturnResultState(result,$"Can not delete accommodation with id: {accommodationId}");
    }

    private void ValidateAccommodationFieldsIfInvalidThrowException(AccommodationEntity accommodation)
    {
        if (accommodation == null || accommodation.UserId == Guid.Empty)
            throw new ArgumentNullException($"{nameof(accommodation)} and {nameof(accommodation.UserId)} can not be null or empty");
    }

    private Result<bool> ValidateNumberOfWrittenDatabaseEntriesAndReturnResultState(int entriesNumber, string errorMessage)
    {
        if (entriesNumber > 0)
        {
            return new Result<bool>(true);
        }

        var error = new InternalDatabaseException(new[] { errorMessage });
        _logger.LogWarning(error, "Internal database error. Can not complete operation");
        return new Result<bool>(error);
    }
}