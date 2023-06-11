using Microsoft.AspNetCore.Identity;

namespace PropertySearchApp.Repositories.Abstract;

public interface IRoleRepository
{
    Task<IdentityRole<Guid>> FindByNameAsync(string name);
}
