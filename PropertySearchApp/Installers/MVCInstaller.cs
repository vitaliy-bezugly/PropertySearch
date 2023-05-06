using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Caching.Memory;
using PropertySearchApp.Common.Constants;
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

        services.ConfigureApplicationCookie(options => options.LoginPath = "/" + ApplicationRoutes.Identity.Login);

        logger.LogInformation("MVC has been successfully installed");
    }
}