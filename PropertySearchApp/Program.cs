using PropertySearchApp;
using PropertySearchApp.Extensions;

var builder = WebApplication.CreateBuilder(args);
ILogger<Startup> logger = builder.CreateLogger<Startup>();

var startup = new Startup(builder.Configuration, logger);
startup.ConfigureServices(builder.Services);

string? port = Environment.GetEnvironmentVariable("PORT");
if(string.IsNullOrEmpty(port) == false)
{
    builder.WebHost.UseUrls($"http://*:{port}");
}

// build application
var app = builder.Build();
startup.Configure(app, app.Environment);
