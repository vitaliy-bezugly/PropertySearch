using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
}
