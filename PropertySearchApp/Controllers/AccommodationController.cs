using System.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertySearchApp.Models;
using PropertySearchApp.Repositories;
using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Controllers;

[Authorize]
public class AccommodationController : Controller
{
    private readonly ILogger<AccommodationRepository> _logger;
    private readonly IAccommodationService _accommodationService;
    private readonly List<AccommodationViewModel> _temporaryAccommodations;
    public AccommodationController(ILogger<AccommodationRepository> logger, IAccommodationService accommodationService)
    {
        _logger = logger;
        _accommodationService = accommodationService;

        _temporaryAccommodations = PrepareData();
    }
    private List<AccommodationViewModel> PrepareData()
    {
        return new List<AccommodationViewModel>
        {
            new AccommodationViewModel
            {
                Id = Guid.Parse("e014e678-54c6-468c-b59c-cd1c03373bdd"),
                Title = "The house 1",
                Description = "Big house 1",
                LandlordEmail = "vitaliy.minaev@gmail.com",
                Price = 450,
                PhotoUri = new Uri("https://i.pinimg.com/originals/31/a7/bd/31a7bd737d28ab08e1be341730100f3e.jpg")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("24f61215-0095-4c21-b56d-2c122325ac31"),
                Title = "The house 2",
                Description = "Big house 2",
                LandlordEmail = "illa.ivasenko@gmail.com",
                Price = 670,
                PhotoUri = new Uri("https://www.tallboxdesign.com/wp-content/uploads/2022/02/transform-a-house-to-a-mansion-1024x683.jpg")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("ef6608ba-8709-401c-975b-d5cbd7545395"),
                Title = "The house 3",
                Description = "Big house 3",
                LandlordEmail = "vyacheslave.zal@gmail.com",
                Price = 789,
                PhotoUri = new Uri("https://beautifuldreamhomeplans.com/Beautiful-Mediterranean-Luxury-Home-Architect-Plans.jpg")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("8fe86923-0bfa-41ad-8b1d-147b00c585cb"),
                Title = "The house 4",
                Description = "Big house 4",
                LandlordEmail = "dmitriy.gera@gmail.com",
                Price = 1200,
                PhotoUri = new Uri("https://www.realestate.com.au/blog/images/1024x768-fit,progressive/2019/08/21114902/capi_89fabccddb1bb4e76ce88070ffd56381_3926fda65b7c535a9f34af1ed1945f37.jpeg")
            },
        };
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(_temporaryAccommodations);
    }
    
    [HttpGet]
    public IActionResult Details(Guid id)
    {
        var accommodation = _temporaryAccommodations.FirstOrDefault(x => x.Id == id);
        return accommodation == null ? NotFound() : View(accommodation);
    }
}
