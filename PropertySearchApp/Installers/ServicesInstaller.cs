using PropertySearchApp.Installers.Abstract;
using PropertySearchApp.Services;
using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Installers;

public class ServicesInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
    {
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IAccommodationService, AccommodationService>();
        services.AddScoped<UserValidatorService>();
        logger.LogInformation("Services are successfully installed");
    }
}