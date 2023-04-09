using Microsoft.AspNetCore.Identity;
using PropertySearchApp.Repositories.Abstract;

namespace PropertySearchApp.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    public RoleRepository(RoleManager<IdentityRole<Guid>> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IdentityRole<Guid>> FindByNameAsync(string name)
    {
        return await _roleManager.FindByNameAsync(name);
    }
}
