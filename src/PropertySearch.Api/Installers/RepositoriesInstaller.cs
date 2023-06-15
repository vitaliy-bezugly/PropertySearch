using PropertySearch.Api.Installers.Abstract;
using PropertySearch.Api.Repositories;
using PropertySearch.Api.Repositories.Abstract;

namespace PropertySearch.Api.Installers;

public class RepositoriesInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
    {
        services.AddScoped<IAccommodationRepository, AccommodationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IContactsRepository, ContactsRepository>();
    }
}