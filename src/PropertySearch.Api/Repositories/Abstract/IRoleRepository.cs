using Microsoft.AspNetCore.Identity;

namespace PropertySearch.Api.Repositories.Abstract;

public interface IRoleRepository : IRepository<IdentityRole<Guid>>
{
    Task<IdentityRole<Guid>> FindByNameAsync(string name);
}
