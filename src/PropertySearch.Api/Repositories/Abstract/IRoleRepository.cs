using Microsoft.AspNetCore.Identity;

namespace PropertySearch.Api.Repositories.Abstract;

public interface IRoleRepository
{
    Task<IdentityRole<Guid>> FindByNameAsync(string name);
}
