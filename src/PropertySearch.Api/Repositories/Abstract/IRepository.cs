namespace PropertySearch.Api.Repositories.Abstract;

public interface IRepository<TEntity>
    where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);

    Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken);

   void Insert(TEntity entity);

   Task DeleteAsync(object id, CancellationToken cancellationToken);

   Task UpdateAsync(TEntity entityToUpdate, CancellationToken cancellationToken);
}