using Microsoft.EntityFrameworkCore;
using PropertySearchApp.Areas.Identity.Data;
using PropertySearchApp.Data;
using PropertySearchApp.Installers.Abstract;

namespace PropertySearchApp.Installers;

public class DatabaseInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
    {
        var connectionString = configuration.GetConnectionString("LocalDatabaseConnection") 
                               ?? throw new InvalidOperationException("Connection string 'LocalDatabaseConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddDefaultIdentity<UserEntity>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        logger.LogInformation("Database has been successfully installed");
    }
}