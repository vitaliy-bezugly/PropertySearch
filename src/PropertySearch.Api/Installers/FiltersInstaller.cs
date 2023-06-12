using PropertySearch.Api.Filters;
using PropertySearch.Api.Installers.Abstract;

namespace PropertySearch.Api.Installers;

public class FiltersInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
    {
        services.AddScoped<LoggingFilter>();
    }
}