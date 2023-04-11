using PropertySearchApp.Installers.Abstract;
using PropertySearchApp.Repositories;
using PropertySearchApp.Repositories.Abstract;

namespace PropertySearchApp.Installers;

public class RepositoriesInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
    {
        services.AddScoped<IAccommodationRepository, AccommodationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserReceiverRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IContactsRepository, ContactsRepository>();
    }
}