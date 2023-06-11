using PropertySearchApp.Installers.Abstract;

namespace PropertySearchApp.Installers.Extensions;

public static class InstallerExtension
{
    public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
    {
        var installers = typeof(Startup).Assembly.ExportedTypes
            .Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IInstaller>()
            .ToList();

        installers.ForEach(installer => installer.InstallService(services, configuration, logger));
    }
}