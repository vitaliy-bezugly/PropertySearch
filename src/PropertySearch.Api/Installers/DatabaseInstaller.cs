using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PropertySearch.Api.Common.Constants;
using PropertySearch.Api.Entities;
using PropertySearch.Api.Installers.Abstract;
using PropertySearch.Api.Persistence;

namespace PropertySearch.Api.Installers;

public class DatabaseInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
    {
        services.AddDbContext<ApplicationDbContext>(GetDatabaseOptions(configuration, logger));

        services.AddDefaultIdentity<UserEntity>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        }).AddRoles<IdentityRole<Guid>>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        services.AddScoped<ApplicationDbContextInitializer>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
    
    private Action<DbContextOptionsBuilder> GetDatabaseOptions(IConfiguration configuration, ILogger<Startup> logger)
    {
        string? connectionString = GetConnectionString(configuration, logger);
        if (Environment.GetEnvironmentVariable(ConnectionNames.InMemory) == "true")
        {
            logger.LogInformation("Using in-memory database");
            return options => options.UseInMemoryDatabase("PropertySearch");
        }

        logger.LogInformation("Using SQL Server database");
        return options => options.UseSqlServer(connectionString, builder => 
            builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
    }
    
    private string GetConnectionString(IConfiguration configuration, ILogger<Startup> logger)
    {
        var connectionString = Environment.GetEnvironmentVariable(ConnectionNames.Environment);
        if (connectionString is not null)
        {
            logger.LogInformation("Received connection string from environment");
            return connectionString;
        }

        logger.LogWarning($"Connection string as environment variable has been not found. " +
                          $"Use appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json");
        connectionString = configuration.GetConnectionString(ConnectionNames.Database);

        if (connectionString is null)
            throw new InvalidOperationException("Connection string has been not found");

        return connectionString;
    }
}
