using Microsoft.AspNetCore.Identity.UI.Services;
using PropertySearchApp.Common.Options;
using PropertySearchApp.Installers.Abstract;
using PropertySearchApp.Services;

namespace PropertySearchApp.Installers;

public class EmailSenderInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
    {
        services.AddTransient<IEmailSender, EmailSenderService>();
        services.Configure<AuthMessageSenderOptions>(configuration);
    }
}