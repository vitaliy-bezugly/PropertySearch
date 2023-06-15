using Microsoft.EntityFrameworkCore;
using PropertySearch.Api.Entities;
using PropertySearch.Api.Persistence;
using PropertySearch.Api.Repositories.Abstract;

namespace PropertySearch.Api.Repositories;

public class AccommodationRepository : RepositoryBase<AccommodationEntity>, IAccommodationRepository
{
    public AccommodationRepository(ApplicationDbContext context) : base(context)
    { }

    public override async Task<AccommodationEntity?> GetByIdAsync(object id, CancellationToken cancellationToken)
    {
        if(id.GetType() != typeof(Guid))
            throw new ArgumentException("Id must be of type Guid.", nameof(id));
        
        return await Table.Include(x => x.Location)
            .FirstOrDefaultAsync(x => x.Id == (Guid)id, cancellationToken);
    }

    public override async Task UpdateAsync(AccommodationEntity entityToUpdate, CancellationToken cancellationToken)
    {
        var source = await base.GetByIdAsync(entityToUpdate.Id, cancellationToken);
        if(source is not null)
        {
            UpdateFields(source, entityToUpdate);
        }
        else
        {
            throw new InvalidOperationException("Can not update accommodation. Accommodation not found.");
        }
    }
    
    public async Task<IEnumerable<AccommodationEntity>> GetWithLimitsAsync(int startAt, int countOfItems, CancellationToken cancellationToken)
    {
        return await Table
            .Include(x => x.Location)
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Skip(startAt)
            .Take(countOfItems)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<AccommodationEntity>> GetUserAccommodationsWithLimitsAsync(Guid userId, int startAt, int countOfItems, CancellationToken cancellationToken)
    {
        return await Table
            .Include(x => x.Location)
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.Id)
            .Skip(startAt)
            .Take(countOfItems)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken)
    {
        return await Table.CountAsync(cancellationToken);
    }
    
    public async Task<int> GetUserAccommodationsCountAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await Table.Where(x => x.UserId == userId).CountAsync(cancellationToken);
    }

    private void UpdateFields(AccommodationEntity source, AccommodationEntity destination)
    {
        source.Title = destination.Title;
        source.Description = destination.Description;
        source.Price = destination.Price;
        source.PhotoUri = destination.PhotoUri;

        if (destination.Location is not null && source.Location is not null)
        {
            source.Location.Country = destination.Location.Country;
            source.Location.City = destination.Location.City;
            source.Location.Region = destination.Location.Region;
            source.Location.Address = destination.Location.Address;
        }
    }
}