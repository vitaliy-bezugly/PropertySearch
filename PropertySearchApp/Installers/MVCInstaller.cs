using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Caching.Memory;
using PropertySearchApp.Installers.Abstract;

namespace PropertySearchApp.Installers;

public class MVCInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
    {
        services.AddControllersWithViews().AddRazorRuntimeCompilation();
        services.AddSingleton<IMemoryCache, MemoryCache>();

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        logger.LogInformation("MVC has been successfully installed");
    }
}