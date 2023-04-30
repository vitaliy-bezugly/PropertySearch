using PropertySearchApp.Installers.Abstract;
using PropertySearchApp.Services;
using PropertySearchApp.Services.Abstract;
using PropertySearchApp.Services.Cached;

namespace PropertySearchApp.Installers;

public class ServicesInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
    {
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IAccommodationService, AccommodationService>();
        services.AddScoped<IUserValidatorService, UserValidatorService>();
        services.AddScoped<ISignInService, SignInService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<ILocationLoadingService, IpInfoLocationLoadingService>();
        services.Decorate<ILocationLoadingService, IpInfoLocationLoadingCachedService>();

        services.AddSingleton<IPInfoClientContainer, DefaultIpInfoClientContainer>();
        services.AddSingleton<IpInfoClientBuilder>();
    }
}