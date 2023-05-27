using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using PropertySearchApp.Common.Extensions;
using PropertySearchApp.Common.Logging;

namespace PropertySearchApp.Filters;

public class LoggingFilter : IAsyncActionFilter
{
    private readonly ILogger<LoggingFilter> _logger;
    public LoggingFilter(ILogger<LoggingFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // This method is called before and after the action method is executed
        // Log the request URL and method
        var log = new LogRequest();

        if (context.HttpContext.Request.Path.Value is not null)
        {
            log.WithPath(context.HttpContext.Request.Path.Value);
        }

        log.WithMethod(context.HttpContext.Request.Method);

        // Log other information such as request headers, user information, etc.
        if (context.HttpContext.User.Identity != null && context.HttpContext.User.Identity.IsAuthenticated)
        {
            log.WithAuthenticatedUser(context.HttpContext.User.Identity?.Name);
        }
        else
        {
            log.WithUnauthenticatedUser();
        }

        var stopwatch = Stopwatch.StartNew();
        
        // Before the action method is executed
        var resultContext = await next();
        
        stopwatch.Stop();

        // After the action method is executed
        // Log the response status code
        log.WithStatusCode(resultContext.HttpContext.Response.StatusCode);

        // Log other information such as execution time, etc.
        log.WithExecutionTime(stopwatch.ElapsedMilliseconds);

        if (context.HttpContext.Response.IsSuccessStatusCode())
        {
            _logger.LogInformation(log.ToString());
        }
        else if (context.HttpContext.Response.IsInternalErrorStatusCode())
        {
            _logger.LogError(log.ToString());
        }
        else
        {
            _logger.LogWarning(log.ToString());
        }
    }
}