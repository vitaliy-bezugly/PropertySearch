using Microsoft.Extensions.Logging.Console;

namespace PropertySearchApp.Extensions;

public static class ApplicationConfigurationHelper
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