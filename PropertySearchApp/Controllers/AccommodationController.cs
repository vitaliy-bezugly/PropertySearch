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
                Description = "Beautifully renovated 2-bedroom apartment in the heart of downtown. Enjoy stunning city views from your private balcony.",
                LandlordEmail = "vitaliy.minaev@gmail.com",
                Price = 450,
                PhotoUri = new Uri("https://i.pinimg.com/originals/31/a7/bd/31a7bd737d28ab08e1be341730100f3e.jpg")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("24f61215-0095-4c21-b56d-2c122325ac31"),
                Title = "The house 2",
                Description = "Cozy 1-bedroom cottage nestled among the trees. Perfect for a romantic getaway or a peaceful retreat.",
                LandlordEmail = "illa.ivasenko@gmail.com",
                Price = 670,
                PhotoUri = new Uri("https://www.tallboxdesign.com/wp-content/uploads/2022/02/transform-a-house-to-a-mansion-1024x683.jpg")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("ef6608ba-8709-401c-975b-d5cbd7545395"),
                Title = "The house 3",
                Description = "Spacious 3-bedroom house with a large backyard and pool. Ideal for families or groups of friends.",
                LandlordEmail = "vyacheslave.zal@gmail.com",
                Price = 789,
                PhotoUri = new Uri("https://beautifuldreamhomeplans.com/Beautiful-Mediterranean-Luxury-Home-Architect-Plans.jpg")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("8fe86923-0bfa-41ad-8b1d-147b00c585cb"),
                Title = "The house 4",
                Description = "Modern studio apartment with all the amenities. Located in a vibrant neighborhood with plenty of restaurants and bars.",
                LandlordEmail = "dmitriy.gera@gmail.com",
                Price = 1200,
                PhotoUri = new Uri("https://www.realestate.com.au/blog/images/1024x768-fit,progressive/2019/08/21114902/capi_89fabccddb1bb4e76ce88070ffd56381_3926fda65b7c535a9f34af1ed1945f37.jpeg")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("8fe86923-0bfa-41ad-8b1d-147b00c585cb"),
                Title = "The house 4",
                Description = "Charming 2-story townhouse with a fireplace and exposed brick walls. Conveniently located near public transportation.",
                LandlordEmail = "dmitriy.gera@gmail.com",
                Price = 1200,
                PhotoUri = new Uri("https://cdn.apartmenttherapy.info/image/upload/v1619013756/at/house%20tours/2021-04/Erin%20K/KERR-130-CLARKSON-2R-01-020577-EDIT-WEB.jpg")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("8fe86923-0bfa-41ad-8b1d-147b00c585cb"),
                Title = "The house 4",
                Description = "Stylish 1-bedroom loft with high ceilings and natural light. Walking distance to shops, cafes, and galleries.",
                LandlordEmail = "dmitriy.gera@gmail.com",
                Price = 1200,
                PhotoUri = new Uri("https://assets.your-move.co.uk/yourmove_new/property-image/rps_yom-WLN200047/image/WLN200047_03/l.jpg")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("8fe86923-0bfa-41ad-8b1d-147b00c585cb"),
                Title = "The house 4",
                Description = "Luxurious 4-bedroom villa with breathtaking ocean views. The perfect place for a tropical escape.",
                LandlordEmail = "dmitriy.gera@gmail.com",
                Price = 1200,
                PhotoUri = new Uri("https://i.ytimg.com/vi/aUHVE3nhFzk/maxresdefault.jpg")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("8fe86923-0bfa-41ad-8b1d-147b00c585cb"),
                Title = "The house 4",
                Description = "Quaint 2-bedroom bungalow with a screened-in porch. Surrounded by lush gardens and located in a quiet neighborhood.",
                LandlordEmail = "dmitriy.gera@gmail.com",
                Price = 1200,
                PhotoUri = new Uri("https://media.onthemarket.com/properties/12957170/1437487200/image-1-480x320.webp")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("8fe86923-0bfa-41ad-8b1d-147b00c585cb"),
                Title = "The house 4",
                Description = "Beautifully furnished 1-bedroom condo with a balcony overlooking the city. Close to parks and museums.",
                LandlordEmail = "dmitriy.gera@gmail.com",
                Price = 1200,
                PhotoUri = new Uri("https://media.onthemarket.com/properties/11857315/1422692974/image-4-480x320.jpg")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("8fe86923-0bfa-41ad-8b1d-147b00c585cb"),
                Title = "The house 4",
                Description = "Cozy 2-bedroom cabin in the woods with a rustic interior and a wood-burning stove. A great spot for nature lovers.",
                LandlordEmail = "dmitriy.gera@gmail.com",
                Price = 1200,
                PhotoUri = new Uri("https://media.onthemarket.com/properties/12722845/1430849693/image-4-480x320.jpg")
            }
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
