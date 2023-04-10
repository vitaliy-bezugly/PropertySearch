using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertySearchApp.Domain;
using PropertySearchApp.Models;
using PropertySearchApp.Repositories;
using PropertySearchApp.Services.Abstract;
using System.Security.Claims;
using PropertySearchApp.Extensions;

namespace PropertySearchApp.Controllers;

[Authorize]
public class AccommodationController : Controller
{
    private readonly ILogger<AccommodationRepository> _logger;
    private readonly IMapper _mapper;
    private readonly IAccommodationService _accommodationService;
    private readonly IIdentityService _identityService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Guid _userId;
    public AccommodationController(ILogger<AccommodationRepository> logger, IAccommodationService accommodationService, IMapper mapper, IHttpContextAccessor httpContextAccessor, IIdentityService identityService)
    {
        _logger = logger;
        _accommodationService = accommodationService;
        _mapper = mapper;

        _httpContextAccessor = httpContextAccessor;
        _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        _identityService = identityService;
    }

    [HttpGet(nameof(Index))]
    public async Task<IActionResult> Index([FromRoute] int id, CancellationToken cancellationToken)
    {
        var accommodations = (await GetAccommodationsWithLimits(id * 12, 12, cancellationToken))
            .Select(x => _mapper.Map<AccommodationViewModel>(x));

        return View(accommodations);
    }
    [HttpGet(nameof(MyOffers))]
    public async Task<IActionResult> MyOffers([FromRoute] int id, CancellationToken cancellationToken)
    {
        var accommodations = (await _accommodationService.GetWithLimitsAsync(id * 12, 12, cancellationToken))
            .Where(x => x.UserId == _userId)
            .Select(x => _mapper.Map<AccommodationViewModel>(x));

        return View("Index", accommodations);
    }
    [HttpGet(nameof(Details) + "/{id}")]
    public async Task<IActionResult> Details([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var accommodation = await _accommodationService.GetAccommodationByIdAsync(id, cancellationToken);
        if (accommodation == null)
            return NotFound();

        var viewModel = _mapper.Map<AccommodationViewModel>(accommodation);
        var account = await _identityService.GetUserByIdAsync(Guid.Parse(viewModel.OwnerId));
        if (account == null)
            return BadRequest("User account that created this offer does not exist");

        viewModel.OwnerUsername = account.Username;
        return View(viewModel);
    }

    [HttpGet(nameof(Create))]
    public IActionResult Create()
    {
        var createAccommodation = new CreateAccommodationViewModel();
        return View(createAccommodation);
    }
    [ValidateAntiForgeryToken, HttpPost(nameof(Create))]
    public async Task<IActionResult> Create(CreateAccommodationViewModel viewModel, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid == false)
            return View(viewModel);

        var accommodation = new AccommodationDomain(Guid.NewGuid(), viewModel.Title, viewModel.Description,
            viewModel.Price, viewModel.PhotoUri, _userId);

        var result = await _accommodationService.CreateAccommodationAsync(accommodation, cancellationToken);

        return result.ToResponse("Successfully created accommodation", TempData, () => View(), () => View(viewModel), (exception, message) => _logger.LogError(exception, message));
    }

    [ValidateAntiForgeryToken, HttpPost(nameof(Delete) + "/{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _accommodationService.DeleteAccommodationAsync(_userId, id, cancellationToken);
        return result.ToResponse("Successfully deleted accommodation", TempData, () => RedirectToAction("Index", "Accommodation"), () => RedirectToAction("Index", "Accommodation"), (exception, message) => _logger.LogError(exception, message));
    }
    [HttpGet(nameof(Update) + "/{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var accommodation = await _accommodationService.GetAccommodationByIdAsync(id, cancellationToken);
        if (accommodation == null)
            return NotFound();

        var updateViewModel = _mapper.Map<UpdateAccommodationViewModel>(accommodation);
        return View(updateViewModel);
    }
    [ValidateAntiForgeryToken, HttpPost(nameof(Update) + "/{id}")]
    public async Task<IActionResult> Update([FromForm] UpdateAccommodationViewModel viewModel, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid == false)
            return View(viewModel);

        var accommodation = new AccommodationDomain(viewModel.Id, viewModel.Title, viewModel.Description,
            viewModel.Price, viewModel.PhotoUri, _userId);

        var result = await _accommodationService.UpdateAccommodationAsync(accommodation, cancellationToken);

        return result.ToResponse("Successfully updated accommodation", TempData, () => View(), () => View(viewModel), (exception, message) => _logger.LogError(exception, message));
    }
    private async Task<IEnumerable<AccommodationDomain>> GetAccommodationsWithLimits(int firstElement, int countOfElements, CancellationToken cancellationToken)
    {
        return await _accommodationService.GetWithLimitsAsync(firstElement, countOfElements, cancellationToken);
    }
}