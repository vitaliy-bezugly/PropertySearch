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
                Title = "A cozy mountain cabin",
                Description = "Beautifully renovated 2-bedroom apartment in the heart of downtown. Enjoy stunning city views from your private balcony.",
                LandlordEmail = "vitaliy.minaev@gmail.com",
                Price = Random.Shared.Next(450, 1480),
                PhotoUri = new Uri("https://i.pinimg.com/originals/31/a7/bd/31a7bd737d28ab08e1be341730100f3e.jpg")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("24f61215-0095-4c21-b56d-2c122325ac31"),
                Title = "A modern apartment with sleek furnishings",
                Description = "Cozy 1-bedroom cottage nestled among the trees. Perfect for a romantic getaway or a peaceful retreat.",
                LandlordEmail = "illa.ivasenko@gmail.com",
                Price = Random.Shared.Next(450, 1480),
                PhotoUri = new Uri("https://www.tallboxdesign.com/wp-content/uploads/2022/02/transform-a-house-to-a-mansion-1024x683.jpg")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("ef6608ba-8709-401c-975b-d5cbd7545395"),
                Title = "A charming beach cottage",
                Description = "Spacious 3-bedroom house with a large backyard and pool. Ideal for families or groups of friends.",
                LandlordEmail = "vyacheslave.zal@gmail.com",
                Price = Random.Shared.Next(450, 1480),
                PhotoUri = new Uri("https://beautifuldreamhomeplans.com/Beautiful-Mediterranean-Luxury-Home-Architect-Plans.jpg")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("8fe86923-0bfa-41ad-8b1d-147b00c585cb"),
                Title = "A luxurious villa",
                Description = "Modern studio apartment with all the amenities. Located in a vibrant neighborhood with plenty of restaurants and bars.",
                LandlordEmail = "dmitriy.gera@gmail.com",
                Price = Random.Shared.Next(450, 1480),
                PhotoUri = new Uri("https://www.realestate.com.au/blog/images/1024x768-fit,progressive/2019/08/21114902/capi_89fabccddb1bb4e76ce88070ffd56381_3926fda65b7c535a9f34af1ed1945f37.jpeg")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("07847d4d-58a8-404c-8c5c-dd80eed41cb8"),
                Title = "Charming 2-story townhouse",
                Description = "Charming 2-story townhouse with a fireplace and exposed brick walls. Conveniently located near public transportation.",
                LandlordEmail = "dmitriy.gera@gmail.com",
                Price = Random.Shared.Next(450, 1480),
                PhotoUri = new Uri("https://cdn.apartmenttherapy.info/image/upload/v1619013756/at/house%20tours/2021-04/Erin%20K/KERR-130-CLARKSON-2R-01-020577-EDIT-WEB.jpg")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("baa9c357-150d-4be3-b7be-e5ea53e16fd0"),
                Title = "A spacious loft with high ceilings and industrial-chic decor",
                Description = "Stylish 1-bedroom loft with high ceilings and natural light. Walking distance to shops, cafes, and galleries.",
                LandlordEmail = "dmitriy.gera@gmail.com",
                Price = Random.Shared.Next(450, 1480),
                PhotoUri = new Uri("https://assets.your-move.co.uk/yourmove_new/property-image/rps_yom-WLN200047/image/WLN200047_03/l.jpg")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("6c02d76d-1514-4eec-a36e-4c5b4b75a580"),
                Title = "A secluded treehouse",
                Description = "Luxurious 4-bedroom villa with breathtaking ocean views. The perfect place for a tropical escape.",
                LandlordEmail = "dmitriy.gera@gmail.com",
                Price = Random.Shared.Next(450, 1480),
                PhotoUri = new Uri("https://i.ytimg.com/vi/aUHVE3nhFzk/maxresdefault.jpg")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("bb338d82-2310-45f9-b8d7-536363bb50e6"),
                Title = "A charming farmhouse",
                Description = "Quaint 2-bedroom bungalow with a screened-in porch. Surrounded by lush gardens and located in a quiet neighborhood.",
                LandlordEmail = "dmitriy.gera@gmail.com",
                Price = Random.Shared.Next(450, 1480),
                PhotoUri = new Uri("https://media.onthemarket.com/properties/12957170/1437487200/image-1-480x320.webp")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("262bf977-a8bb-4444-ab6c-d73c4628cca3"),
                Title = "A traditional Japanese ryokan",
                Description = "Beautifully furnished 1-bedroom condo with a balcony overlooking the city. Close to parks and museums.",
                LandlordEmail = "dmitriy.gera@gmail.com",
                Price = Random.Shared.Next(450, 1480),
                PhotoUri = new Uri("https://media.onthemarket.com/properties/11857315/1422692974/image-4-480x320.jpg")
            },
            new AccommodationViewModel
            {
                Id = Guid.Parse("cf743825-d8d8-47f5-8c1d-6c514b23012d"),
                Title = "A boutique hotel",
                Description = "Cozy 2-bedroom cabin in the woods with a rustic interior and a wood-burning stove. A great spot for nature lovers.",
                LandlordEmail = "dmitriy.gera@gmail.com",
                Price = Random.Shared.Next(450, 1480),
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
