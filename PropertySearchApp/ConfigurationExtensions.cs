using PropertySearchApp.Common.Constants;

namespace PropertySearchApp;

public static class ConfigurationExtensions
{
    public static void Configure404Error(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            await next();

            if ((context.Response.StatusCode == StatusCodes.Status404NotFound) 
                && !context.Response.HasStarted)
            {
                //Re-execute the request so the user gets the error page
                string originalPath = context.Request.Path.Value;
                context.Items["originalPath"] = originalPath;
                context.Request.Path = "/" + ApplicationRoutes.Error.NotFound;
                await next();
            }
        });
    }
}
