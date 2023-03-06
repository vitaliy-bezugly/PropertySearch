using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertySearchApp.Repositories;
using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Controllers;

[Authorize]
public class AccommodationController : Controller
{
    private readonly ILogger<AccommodationRepository> _logger;
    private readonly IAccommodationService _accommodationService;

    public AccommodationController(ILogger<AccommodationRepository> logger, IAccommodationService accommodationService)
    {
        _logger = logger;
        _accommodationService = accommodationService;
    }

    public IActionResult Index()
    {
        return View();
    }
}
