using Microsoft.EntityFrameworkCore;
using PropertySearch.Api.Persistence;

namespace PropertySearch.Api.Repositories.Abstract;

public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> Table;
    
    protected RepositoryBase(ApplicationDbContext context)
    {
        _context = context;
        Table = _context.Set<TEntity>();
    }
    
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await Table.ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken)
    {
        return await Table.FindAsync(new[]{id}, cancellationToken);
    }

    public virtual void Insert(TEntity entity)
    {
        Table.Add(entity);
    }

    public virtual async Task DeleteAsync(object id, CancellationToken cancellationToken)
    {
        TEntity? entityToDelete = await this.GetByIdAsync(id, cancellationToken);
        if (entityToDelete is null)
            return;
        
        Delete(entityToDelete);
    }

    protected virtual void Delete(TEntity entityToDelete)
    {
        if (_context.Entry(entityToDelete).State == EntityState.Detached)
        {
            Table.Attach(entityToDelete);
        }
        
        Table.Remove(entityToDelete);
    }

    public virtual Task UpdateAsync(TEntity entityToUpdate, CancellationToken cancellationToken)
    {
        Table.Attach(entityToUpdate);
        _context.Entry(entityToUpdate).State = EntityState.Modified;
        return Task.CompletedTask;
    }
}