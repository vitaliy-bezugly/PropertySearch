using PropertySearchApp.Common.Constants;
using PropertySearchApp.ConfigurationExtensions;
using PropertySearchApp.Installers.Extensions;

namespace PropertySearchApp;

public class Startup
{
    private readonly ILogger<Startup> _logger;
    private readonly IConfiguration _configuration;
    
    public Startup(IConfiguration configuration, ILogger<Startup> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // install services via reflection 
        services.InstallServicesInAssembly(_configuration, _logger);
    }
    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        
        app.Configure404Error();
        
        // For ip address getting
        app.UseForwardedHeaders();
        
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseDefaultFiles();

        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.UseCors(Policy.Name);

        app.MapRazorPages();
        
        // views -> error -> servererror.cshtml
        app.UseExceptionHandler("/" + ApplicationRoutes.Error.InternalServerError);

        app.Run();
    }
}
