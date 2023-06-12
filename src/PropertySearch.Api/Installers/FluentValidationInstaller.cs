using FluentValidation;
using PropertySearch.Api.Common;
using PropertySearch.Api.Installers.Abstract;

namespace PropertySearch.Api.Installers;

public class FluentValidationInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
    {
        services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();
    }
}
