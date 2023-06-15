using Microsoft.EntityFrameworkCore;
using PropertySearch.Api.Repositories;
using PropertySearch.Api.Repositories.Abstract;

namespace PropertySearch.Api.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private IAccommodationRepository? _accommodationRepository;
    
    private bool _disposed;
    
    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public void Rollback()
    {
        foreach (var entry in _dbContext.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
            }
        }
    }

    public IAccommodationRepository AccommodationRepository
    {
        get
        {
            if(_accommodationRepository is null)
            {
                _accommodationRepository = new AccommodationRepository(_dbContext);
            }

            return _accommodationRepository;
        }
    }

    private void Dispose(bool disposing)
    {
        if (_disposed == false)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
        
        _disposed = true;
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}