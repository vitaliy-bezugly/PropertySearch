using PropertySearchApp.Installers.Abstract;

namespace PropertySearchApp.Installers;

public class MVCInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
    {
        services.AddControllersWithViews();
    }
}