using Microsoft.AspNetCore.Identity;
using PropertySearchApp.Persistence.Exceptions;

namespace PropertySearchApp.Persistence.Extensions;

public static class DatabasePreparationExtension
{
    public static async Task AddRolesInDatabaseAsync(this WebApplication application, ILogger logger, List<string> requiredRoles)
    {
        using (var scope = application.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            foreach (var item in requiredRoles)
            {
                IdentityRole<Guid> role = await roleManager.FindByNameAsync(item);

                if (role == null)
                {
                    var result = await AddRoleToDatabaseAsync(item, roleManager);

                    if(result.Succeeded)
                    {
                        logger.LogInformation($"Successfully added {item} role to table");
                    }
                    else
                    {
                        var exception = new RoleCreationException(result.Errors.Select(x => x.Description));

                        logger.LogCritical(exception, $"Can not create role: {item}");
                        throw exception;
                    }
                }
            }
        }
    }

    private static async Task<IdentityResult> AddRoleToDatabaseAsync(string roleName, RoleManager<IdentityRole<Guid>> roleManager)
    {
        return await roleManager.CreateAsync(new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = roleName });
    }
}
