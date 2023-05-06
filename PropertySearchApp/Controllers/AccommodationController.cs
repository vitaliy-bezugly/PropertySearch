using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertySearchApp.Domain;
using PropertySearchApp.Repositories;
using PropertySearchApp.Services.Abstract;
using PropertySearchApp.Common.Constants;
using PropertySearchApp.Controllers.Extensions;
using System.Net;
using PropertySearchApp.Common.Extensions;
using PropertySearchApp.Models.Accommodations;
using PropertySearchApp.Models.Location;

namespace PropertySearchApp.Controllers;

[Authorize]
public class AccommodationController : Controller
{
    private readonly ILogger<AccommodationRepository> _logger;
    private readonly IMapper _mapper;
    private readonly IAccommodationService _accommodationService;
    private readonly IIdentityService _identityService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILocationLoadingService _locationService;
    public AccommodationController(ILogger<AccommodationRepository> logger, IAccommodationService accommodationService, IMapper mapper, IHttpContextAccessor httpContextAccessor, IIdentityService identityService, ILocationLoadingService locationService)
    {
        _logger = logger;
        _accommodationService = accommodationService;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _identityService = identityService;
        _locationService = locationService;
    }

    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> Index([FromRoute] int? id, CancellationToken cancellationToken)
    {
        var pageId = id == null ? 0 : id.Value;
        var accommodations = (await GetAccommodationsWithLimits(pageId, 64, cancellationToken))
            .Select(x => _mapper.Map<AccommodationViewModel>(x));

        return View(accommodations);
    }
    [HttpGet, Route(ApplicationRoutes.Accommodation.MyOffers)]
    public async Task<IActionResult> MyOffers([FromRoute] int? id, CancellationToken cancellationToken)
    {
        var pageId = id == null ? 0 : id.Value;
        Guid userId = _httpContextAccessor.GetUserId();

        var accommodations = (await _accommodationService.GetWithLimitsAsync(pageId, 64, cancellationToken))
            .Where(x => x.UserId == userId)
            .Select(x => _mapper.Map<AccommodationViewModel>(x));

        return View("Index", accommodations);
    }
    [HttpGet, Route(ApplicationRoutes.Accommodation.Details)]
    public async Task<IActionResult> Details([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var accommodation = await _accommodationService.GetAccommodationByIdAsync(id, cancellationToken);
        if (accommodation == null)
            return RedirectToAction("PageNotFound", ApplicationRoutes.Error.Base);

        var viewModel = _mapper.Map<AccommodationViewModel>(accommodation);
        var account = await _identityService.GetUserByIdAsync(Guid.Parse(viewModel.OwnerId));
        if (account == null)
            return BadRequest("User account that created this offer does not exist");

        viewModel.OwnerUsername = account.Username;
        return View(viewModel);
    }

    [HttpGet, Route(ApplicationRoutes.Accommodation.Create)]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        IPAddress? remoteIp = Request.HttpContext.Connection.RemoteIpAddress;

        var location = await _locationService.GetLocationByUrlAsync(remoteIp, cancellationToken);
        var createAccommodationModel = new CreateAccommodationViewModel
        {
            Location = _mapper.Map<LocationViewModel>(location)
        };
        return View(createAccommodationModel);
    }
    [HttpPost, ValidateAntiForgeryToken, Route(ApplicationRoutes.Accommodation.Create)]
    public async Task<IActionResult> Create(CreateAccommodationViewModel viewModel, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid == false)
            return View(viewModel);

        Guid userId = _httpContextAccessor.GetUserId();

        var location = new LocationDomain { Id = Guid.NewGuid(), Country = viewModel.Location.Country, Region = viewModel.Location.Region, City = viewModel.Location.City, Address = viewModel.Location.Address };    
        var accommodation = new AccommodationDomain(Guid.NewGuid(), viewModel.Title, viewModel.Description,
            viewModel.Price, viewModel.PhotoUri, userId, location);

        var result = await _accommodationService.CreateAccommodationAsync(accommodation, cancellationToken);
        return result.ToResponse(SuccessMessages.Accommodation.Created, TempData, 
            () => View(), 
            () => View(viewModel));
    }
    
    [HttpPost, Route(ApplicationRoutes.Accommodation.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        Guid userId = _httpContextAccessor.GetUserId();
        
        var result = await _accommodationService.DeleteAccommodationAsync(userId, id, cancellationToken);
        return result.ToResponse(SuccessMessages.Accommodation.Deleted, TempData, 
            () => RedirectToAction("Index", "Accommodation"), 
            () => RedirectToAction("Index", "Accommodation"));
    }
    
    [HttpGet, Route(ApplicationRoutes.Accommodation.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var accommodation = await _accommodationService.GetAccommodationByIdAsync(id, cancellationToken);
        if (accommodation == null)
            return NotFound();

        var updateViewModel = _mapper.Map<UpdateAccommodationViewModel>(accommodation);
        return View(updateViewModel);
    }
    [HttpPost, ValidateAntiForgeryToken, Route(ApplicationRoutes.Accommodation.Update)]
    public async Task<IActionResult> Update([FromForm] UpdateAccommodationViewModel viewModel, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid == false)
            return View(viewModel);
        
        var userId = _httpContextAccessor.GetUserId();
        
        var location = new LocationDomain { Id = Guid.NewGuid(), Country = viewModel.Location.Country, Region = viewModel.Location.Region, City = viewModel.Location.City, Address = viewModel.Location.Address };
        var accommodation = new AccommodationDomain(viewModel.Id, viewModel.Title, viewModel.Description,
            viewModel.Price, viewModel.PhotoUri, userId, location);

        var result = await _accommodationService.UpdateAccommodationAsync(accommodation, cancellationToken);

        return result.ToResponse(SuccessMessages.Accommodation.Updated, TempData, 
            () => View(), 
            () => View(viewModel));
    }

    private async Task<IEnumerable<AccommodationDomain>> GetAccommodationsWithLimits(int pageId, int countOfElements, CancellationToken cancellationToken)
    {
        return await _accommodationService.GetWithLimitsAsync(pageId * countOfElements, countOfElements, cancellationToken);
    }
}