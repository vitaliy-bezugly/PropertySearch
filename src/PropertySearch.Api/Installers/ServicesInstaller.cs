using Microsoft.AspNetCore.Identity.UI.Services;
using PropertySearch.Api.Common.Options;
using PropertySearch.Api.Installers.Abstract;
using PropertySearch.Api.Repositories;
using PropertySearch.Api.Repositories.Abstract;
using PropertySearch.Api.Services;
using PropertySearch.Api.Services.Abstract;
using PropertySearch.Api.Services.Cached;

namespace PropertySearch.Api.Installers;

public class ServicesInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
    {
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IUserValidatorService, UserValidatorService>();
        services.AddScoped<ITokenProvider, UserTokenProvider>();
        services.AddScoped<ISignInService, SignInService>();

        services.AddScoped<IAccommodationService, AccommodationService>();

        services.AddScoped<IContactService, ContactService>();

        services.Configure<IpInfoOptions>(configuration);
        services.AddSingleton<IPInfoClientContainer, IpInfoClientContainer>();
        
        services.AddScoped<ILocationLoadingService, LocationLoadingService>();
        services.Decorate<ILocationLoadingService, IpInfoLocationLoadingCachedService>();
        
        services.AddTransient<IEmailSender, EmailSenderService>();
        services.Configure<AuthMessageSenderOptions>(configuration);

        services.AddSingleton<UrlBuilder>();
        services.AddScoped<IHtmlMessageBuilder, HtmlMessageBuilder>();
    }
}