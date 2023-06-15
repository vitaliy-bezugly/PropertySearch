using PropertySearch.Api.Common;
using PropertySearch.Api.Entities;

namespace PropertySearch.Api.Repositories.Abstract;

public interface IContactsRepository : IRepository<ContactEntity>
{
    Task<List<ContactEntity>> GetUserContactsAsync(Guid userId);
}
