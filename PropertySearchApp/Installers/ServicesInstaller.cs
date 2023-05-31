using Microsoft.AspNetCore.Identity.UI.Services;
using PropertySearchApp.Common.Options;
using PropertySearchApp.Installers.Abstract;
using PropertySearchApp.Repositories;
using PropertySearchApp.Repositories.Abstract;
using PropertySearchApp.Services;
using PropertySearchApp.Services.Abstract;
using PropertySearchApp.Services.Cached;

namespace PropertySearchApp.Installers;

public class ServicesInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
    {
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IUserValidatorService, UserValidatorService>();
        services.AddScoped<IUserTokenProvider, UserRepository>();
        services.AddScoped<ISignInService, SignInService>();

        services.AddScoped<IAccommodationService, AccommodationService>();

        services.AddScoped<IContactService, ContactService>();

        services.Configure<IpInfoOptions>(configuration);
        services.AddSingleton<IpInfoClientBuilder>();
        services.AddSingleton<IPInfoClientContainer, DefaultIpInfoClientContainer>();
        
        services.AddScoped<ILocationLoadingService, IpInfoLocationLoadingService>();
        services.Decorate<ILocationLoadingService, IpInfoLocationLoadingCachedService>();
        
        services.AddTransient<IEmailSender, EmailSenderService>();
        services.Configure<AuthMessageSenderOptions>(configuration);

        services.AddSingleton<UrlBuilder>();
        services.AddScoped<IHtmlMessageBuilder, HtmlMessageBuilder>();
    }
}