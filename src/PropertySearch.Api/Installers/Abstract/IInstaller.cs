namespace PropertySearch.Api.Installers.Abstract;

public interface IInstaller
{
    void InstallService(IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger);
}