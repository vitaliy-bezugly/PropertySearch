using Microsoft.AspNetCore.Identity;
using PropertySearch.Api.Common.Logging;
using PropertySearch.Api.Repositories.Abstract;
using PropertySearch.Api.Common.Extensions;

namespace PropertySearch.Api.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly ILogger<RoleRepository> _logger;
    public RoleRepository(RoleManager<IdentityRole<Guid>> roleManager, ILogger<RoleRepository> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<IdentityRole<Guid>> FindByNameAsync(string name)
    {
        try
        {
            return await _roleManager.FindByNameAsync(name);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(RoleRepository))
                .WithMethod(nameof(FindByNameAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(nameof(String), nameof(name), name)
                .ToString());
            
            throw;
        }
    }
}
