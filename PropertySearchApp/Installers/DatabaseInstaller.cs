using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PropertySearchApp.Entities;
using PropertySearchApp.Installers.Abstract;
using PropertySearchApp.Persistence;

namespace PropertySearchApp.Installers;

public class DatabaseInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
    {


        var connectionString = GetConnString(configuration);

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
        })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
    }
    private string GetConnString(IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONN");
        if (connectionString != null) return connectionString;

        connectionString = configuration.GetConnectionString("Production")
                           ?? throw new InvalidOperationException("Connection string not found.");

        return connectionString;
    }
}
