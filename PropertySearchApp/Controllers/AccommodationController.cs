using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertySearchApp.Domain;
using PropertySearchApp.Models;
using PropertySearchApp.Repositories;
using PropertySearchApp.Services.Abstract;
using System.Security.Claims;

namespace PropertySearchApp.Controllers;

[Authorize]
public class AccommodationController : Controller
{
    private readonly ILogger<AccommodationRepository> _logger;
    private readonly IMapper _mapper;
    private readonly IAccommodationService _accommodationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Guid _userId;
    public AccommodationController(ILogger<AccommodationRepository> logger, IAccommodationService accommodationService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _accommodationService = accommodationService;
        _mapper = mapper;

        _httpContextAccessor = httpContextAccessor;
        _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var accommodations = (await _accommodationService.GetAccommodationsAsync(cancellationToken))
            .Select(x => _mapper.Map<AccommodationViewModel>(x));

        return View(accommodations);
    }
    [HttpGet]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var accommodation = await _accommodationService.GetAccommodationByIdAsync(id, cancellationToken);
        return accommodation == null ? NotFound() : View(_mapper.Map<AccommodationViewModel>(accommodation));
    }
    [HttpGet]
    public IActionResult Create()
    {
        var createAccommodation = new CreateAccommodationViewModel();
        return View(createAccommodation);
    }
    [ValidateAntiForgeryToken, HttpPost]
    public async Task<IActionResult> Create(CreateAccommodationViewModel viewModel, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid == false)
            return View(viewModel);

        // TODO - create ctor
        var accommodation = new AccommodationDomain 
        { 
            Id = Guid.NewGuid(), 
            Title = viewModel.Title,
            Description = viewModel.Description,
            Price = viewModel.Price,
            PhotoUri = viewModel.PhotoUri,
            UserId = _userId
        };

        var result = await _accommodationService.CreateAccommodationAsync(accommodation, cancellationToken);

        return result.Match<IActionResult>(success =>
        {
            return RedirectToAction("Index", "Accommodation");
        }, exception =>
        {
            throw new NotImplementedException();
        });
    }
}
