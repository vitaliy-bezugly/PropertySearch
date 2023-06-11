using PropertySearchApp.Installers.Abstract;

namespace PropertySearchApp.Installers;

public class AutoMapperInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
    {
        services.AddAutoMapper(typeof(Startup));
    }
}