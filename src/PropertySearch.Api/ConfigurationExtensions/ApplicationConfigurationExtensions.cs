using Microsoft.Extensions.Logging.Console;

namespace PropertySearch.Api.ConfigurationExtensions;

public static class ApplicationConfigurationExtension
{
    public static ILogger<T> CreateLogger<T>(this WebApplicationBuilder builder)
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddSimpleConsole(i => i.ColorBehavior = LoggerColorBehavior.Enabled);
        });
        var logger = loggerFactory.CreateLogger<T>();
        
        return logger;
    }
}