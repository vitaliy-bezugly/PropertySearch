using PropertySearch.Api.Repositories.Abstract;

namespace PropertySearch.Api.Persistence;

public interface IUnitOfWork: IDisposable
{
    Task CommitAsync(CancellationToken cancellationToken);
    void Rollback();

    public IAccommodationRepository AccommodationRepository { get; }
    public IContactsRepository ContactsRepository { get; }
    public IRoleRepository RoleRepository { get; }
    public IUserRepository UserRepository { get; }
}