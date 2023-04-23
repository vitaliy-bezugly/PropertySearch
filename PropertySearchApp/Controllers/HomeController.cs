﻿using Microsoft.AspNetCore.Mvc;
using PropertySearchApp.Models;
using System.Diagnostics;

namespace PropertySearchApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        _logger.LogInformation("Request to: " + nameof(HomeController) + "; method: " + nameof(Index));
        return View();
    }

    public IActionResult Privacy()
    {
        _logger.LogInformation("Request to: " + nameof(HomeController) + "; method: " + nameof(Privacy));
        return View();
    }

    public IActionResult Team()
    {
        _logger.LogInformation("Request to: " + nameof(HomeController) + "; method: " + nameof(Team));
        return View();
    }

    public IActionResult Contacts()
    {
        _logger.LogInformation("Request to: " + nameof(HomeController) + "; method: " + nameof(Contacts));
        return View();
    }

    public IActionResult About()
    {
        _logger.LogInformation("Request to: " + nameof(HomeController) + "; method: " + nameof(About));
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}