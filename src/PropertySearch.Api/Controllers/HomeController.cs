using Microsoft.AspNetCore.Mvc;
using PropertySearch.Api.Models;
using System.Diagnostics;
using PropertySearch.Api.Common.Logging;
using PropertySearch.Api.Filters;
using PropertySearch.Api.Common.Extensions;

namespace PropertySearch.Api.Controllers;

[ServiceFilter(typeof(LoggingFilter))]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        try
        {
            return View();
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(HomeController))
                .WithMethod(nameof(Index))
                .WithOperation(nameof(HttpGetAttribute))
                .WithNoParameters()
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }

    [HttpGet]
    public IActionResult Privacy()
    {
        try
        {
            return View();
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(HomeController))
                .WithMethod(nameof(Privacy))
                .WithOperation(nameof(HttpGetAttribute))
                .WithNoParameters()
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }

    [HttpGet]
    public IActionResult Team()
    {
        try
        {
            return View();
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(HomeController))
                .WithMethod(nameof(Team))
                .WithOperation(nameof(HttpGetAttribute))
                .WithNoParameters()
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }

    [HttpGet]
    public IActionResult Contacts()
    {
        try
        {
            return View();
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(HomeController))
                .WithMethod(nameof(Contacts))
                .WithOperation(nameof(HttpGetAttribute))
                .WithNoParameters()
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }

    [HttpGet]
    public IActionResult About()
    {
        try
        {
            return View();
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(HomeController))
                .WithMethod(nameof(About))
                .WithOperation(nameof(HttpGetAttribute))
                .WithNoParameters()
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }
}