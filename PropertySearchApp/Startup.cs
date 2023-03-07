using PropertySearchApp.Installers.Extensions;
using PropertySearchApp.Persistence.Extensions;

namespace PropertySearchApp;

public class Startup
{
    private readonly ILogger<Startup> _logger;
    private readonly List<string> _requiredRoles;
    private const bool isUpToDate = false;
    public IConfiguration Configuration
    {
        get;
    }
    public Startup(IConfiguration configuration, ILogger<Startup> logger)
    {
        this.Configuration = configuration;
        _logger = logger;

        _requiredRoles = new List<string> { "user", "admin", "landlord" };
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.InstallServicesInAssembly(Configuration, _logger);
    }
    public async Task ConfigureAsync(WebApplication app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();;

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        if(isUpToDate == false)
            await app.AddRolesInDatabaseAsync(app.Logger, _requiredRoles);

        app.Run();
    }
}