using Microsoft.AspNetCore.Identity;
using PropertySearch.Api.Repositories.Abstract;
using PropertySearch.Api.Persistence;

namespace PropertySearch.Api.Repositories;

public class RoleRepository : RepositoryBase<IdentityRole<Guid>>, IRoleRepository
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    
    public RoleRepository(RoleManager<IdentityRole<Guid>> roleManager, ApplicationDbContext context) : base(context)
    {
        _roleManager = roleManager;
    }

    public async Task<IdentityRole<Guid>> FindByNameAsync(string name)
    {
        return await _roleManager.FindByNameAsync(name);
    }
}
