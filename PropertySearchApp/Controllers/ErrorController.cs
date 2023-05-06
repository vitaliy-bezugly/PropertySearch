using Microsoft.AspNetCore.Mvc;
using PropertySearchApp.Common.Constants;

namespace PropertySearchApp.Controllers;

public class ErrorController : Controller
{
    private readonly ILogger<ErrorController> _logger;
    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    [HttpGet, Route(ApplicationRoutes.Error.NotFound)]
    public IActionResult PageNotFound()
    {
        string originalPath = "unknown";
        if (HttpContext.Items.ContainsKey("originalPath"))
        {
            originalPath = HttpContext.Items["originalPath"] as string;
        }

        _logger.LogWarning("404 error has been invoked, origin path: " +  originalPath);
        return View();
    }
}
