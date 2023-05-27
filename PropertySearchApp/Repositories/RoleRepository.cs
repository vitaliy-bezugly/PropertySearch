using Microsoft.AspNetCore.Identity;
using PropertySearchApp.Common.Extensions;
using PropertySearchApp.Common.Logging;
using PropertySearchApp.Repositories.Abstract;

namespace PropertySearchApp.Repositories;

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
                .WithParameter(typeof(string).Name, nameof(name), name)
                .ToString());
            
            throw;
        }
    }
}
