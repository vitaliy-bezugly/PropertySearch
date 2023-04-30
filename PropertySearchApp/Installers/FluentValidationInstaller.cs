using FluentValidation;
using PropertySearchApp.Common;
using PropertySearchApp.Installers.Abstract;

namespace PropertySearchApp.Installers;

public class FluentValidationInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
    {
        services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();
    }
}
