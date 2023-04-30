using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PropertySearchApp.Common.Constants;
using PropertySearchApp.Entities;
using PropertySearchApp.Installers.Abstract;
using PropertySearchApp.Persistence;

namespace PropertySearchApp.Installers;

public class DatabaseInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
    {
        var connectionString = GetConnectionString(configuration, logger);

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddDefaultIdentity<UserEntity>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        }).AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<ApplicationDbContext>();
    }
    private string GetConnectionString(IConfiguration configuration, ILogger<Startup> logger)
    {
        var connectionString = Environment.GetEnvironmentVariable(ConnectionNames.Environment);
        if (connectionString != null)
        {
            logger.LogInformation("Received connection string from environment");
            return connectionString;
        }

        logger.LogWarning("Connection string as environment variable has been not found. Use appsetting.json");
        connectionString = configuration.GetConnectionString(ConnectionNames.Docker)
                           ?? throw new InvalidOperationException("Connection string not found.");

        return connectionString;
    }
}
