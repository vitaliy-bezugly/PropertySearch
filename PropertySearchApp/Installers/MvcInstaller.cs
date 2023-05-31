using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using PropertySearchApp.Common.Constants;
using PropertySearchApp.Installers.Abstract;

namespace PropertySearchApp.Installers;

public class MvcInstaller : IInstaller
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

        services.AddCors(options =>
        {
            options.AddPolicy(Policy.Name, policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        
        // email sending features
        services.ConfigureApplicationCookie(o => {
            o.ExpireTimeSpan = TimeSpan.FromDays(5);
            o.SlidingExpiration = true;
        });
        services.Configure<DataProtectionTokenProviderOptions>(o =>
            o.TokenLifespan = TimeSpan.FromHours(3));
    }
}