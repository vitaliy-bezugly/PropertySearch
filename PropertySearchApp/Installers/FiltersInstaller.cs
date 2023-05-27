using PropertySearchApp.Filters;
using PropertySearchApp.Installers.Abstract;

namespace PropertySearchApp.Installers;

public class FiltersInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
    {
        services.AddScoped<LoggingFilter>();
    }
}