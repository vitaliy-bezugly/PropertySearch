using IPinfo;
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
        services.AddScoped<IUserValidatorService, UserValidatorService>();
        services.AddScoped<ISignInService, SignInService>();
        services.AddScoped<IContactsService, ContactsService>();
        services.AddScoped<ILocationLoadingService, IpInfoLocationLoadingService>();

        services.AddSingleton<IPInfoClientContainer, DefaultIpInfoClientContainer>();
        services.AddSingleton<IpInfoClientBuilder>();
    }
}