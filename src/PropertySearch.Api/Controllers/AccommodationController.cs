using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertySearch.Api.Controllers.Extensions;
using System.Net;
using PropertySearch.Api.Common.Constants;
using PropertySearch.Api.Common.Logging;
using PropertySearch.Api.Domain;
using PropertySearch.Api.Filters;
using PropertySearch.Api.Models.Accommodations;
using PropertySearch.Api.Models.Location;
using PropertySearch.Api.Services.Abstract;
using PropertySearch.Api.Common.Extensions;

namespace PropertySearch.Api.Controllers;

[Authorize]
[ServiceFilter(typeof(LoggingFilter))]
public class AccommodationController : Controller
{
    private readonly ILogger<AccommodationController> _logger;
    private readonly IMapper _mapper;
    private readonly IAccommodationService _accommodationService;
    private readonly IIdentityService _identityService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILocationLoadingService _locationService;
    public AccommodationController(ILogger<AccommodationController> logger, IAccommodationService accommodationService, IMapper mapper, IHttpContextAccessor httpContextAccessor, IIdentityService identityService, ILocationLoadingService locationService)
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
        try
        {
            var pageId = id == null ? 0 : id.Value;
            var accommodations = (await GetAccommodationsWithLimits(pageId, 64, cancellationToken))
                .Select(x => _mapper.Map<AccommodationViewModel>(x));

            return View(accommodations);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(AccommodationController))
                .WithMethod(nameof(Index))
                .WithOperation(nameof(HttpGetAttribute))
                .WithParameter(typeof(int?).FullName ?? String.Empty, nameof(id), id.ToString() ?? String.Empty)
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }
    [HttpGet, Route(ApplicationRoutes.Accommodation.MyOffers)]
    public async Task<IActionResult> MyOffers([FromRoute] int? id, CancellationToken cancellationToken)
    {
        try
        {
            var pageId = id == null ? 0 : id.Value;
            Guid userId = _httpContextAccessor.GetUserId();

            var accommodations = (await _accommodationService.GetWithLimitsAsync(pageId, 64, cancellationToken))
                .Where(x => x.UserId == userId)
                .Select(x => _mapper.Map<AccommodationViewModel>(x));

            return View("Index", accommodations);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(AccommodationController))
                .WithMethod(nameof(MyOffers))
                .WithOperation(nameof(HttpGetAttribute))
                .WithParameter(typeof(int?).FullName ?? String.Empty, nameof(id), id.ToString() ?? String.Empty)
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }
    [HttpGet, Route(ApplicationRoutes.Accommodation.Details)]
    public async Task<IActionResult> Details([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
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
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(AccommodationController))
                .WithMethod(nameof(Details))
                .WithOperation(nameof(HttpGetAttribute))
                .WithParameter(typeof(Guid).FullName ?? String.Empty, nameof(id), id.ToString())
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }

    [HttpGet, Route(ApplicationRoutes.Accommodation.Create)]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        try
        {
            IPAddress? remoteIp = Request.HttpContext.Connection.RemoteIpAddress;

            if (remoteIp is null)
                return View(new CreateAccommodationViewModel());

            var location = await _locationService.GetLocationByUrlAsync(remoteIp, cancellationToken);
            var createAccommodationModel = new CreateAccommodationViewModel
            {
                Location = _mapper.Map<LocationViewModel>(location)
            };
            return View(createAccommodationModel);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(AccommodationController))
                .WithMethod(nameof(Create))
                .WithOperation(nameof(HttpGetAttribute))
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }
    
    [HttpPost, ValidateAntiForgeryToken, Route(ApplicationRoutes.Accommodation.Create)]
    public async Task<IActionResult> Create(CreateAccommodationViewModel viewModel, CancellationToken cancellationToken)
    {
        try
        {
            if (ModelState.IsValid == false)
                return View(viewModel);

            Guid userId = _httpContextAccessor.GetUserId();
            
            var accommodation = _mapper.Map<AccommodationDomain>(viewModel);
            accommodation.UserId = userId;

            var result = await _accommodationService.CreateAccommodationAsync(accommodation, cancellationToken);
            return result.ToResponse(SuccessMessages.Accommodation.Created, TempData, 
                () => View(), 
                () => View(viewModel));
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(AccommodationController))
                .WithMethod(nameof(Create))
                .WithOperation(nameof(HttpPostAttribute))
                .WithParameter(typeof(CreateAccommodationViewModel).FullName ?? String.Empty, nameof(viewModel), viewModel.SerializeObject())
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }
    
    [HttpPost, Route(ApplicationRoutes.Accommodation.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            Guid userId = _httpContextAccessor.GetUserId();
        
            var result = await _accommodationService.DeleteAccommodationAsync(userId, id, cancellationToken);
            return result.ToResponse(SuccessMessages.Accommodation.Deleted, TempData, 
                () => RedirectToAction("Index", "Accommodation"), 
                () => RedirectToAction("Index", "Accommodation"));
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(AccommodationController))
                .WithMethod(nameof(Delete))
                .WithOperation(nameof(HttpPostAttribute))
                .WithParameter(typeof(Guid).FullName ?? String.Empty, nameof(id), id.ToString())
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }
    
    [HttpGet, Route(ApplicationRoutes.Accommodation.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var accommodation = await _accommodationService.GetAccommodationByIdAsync(id, cancellationToken);
            if (accommodation == null)
                return NotFound();

            var updateViewModel = _mapper.Map<UpdateAccommodationViewModel>(accommodation);
            return View(updateViewModel);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(AccommodationController))
                .WithMethod(nameof(Update))
                .WithOperation(nameof(HttpGetAttribute))
                .WithParameter(typeof(Guid).FullName ?? String.Empty, nameof(id), id.ToString())
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }
    [HttpPost, ValidateAntiForgeryToken, Route(ApplicationRoutes.Accommodation.Update)]
    public async Task<IActionResult> Update([FromForm] UpdateAccommodationViewModel viewModel, CancellationToken cancellationToken)
    {
        try
        {
            if (ModelState.IsValid == false)
                return View(viewModel);
        
            var userId = _httpContextAccessor.GetUserId();
            
            var accommodation = _mapper.Map<AccommodationDomain>(viewModel);
            accommodation.UserId = userId;

            var result = await _accommodationService.UpdateAccommodationAsync(accommodation, cancellationToken);

            return result.ToResponse(SuccessMessages.Accommodation.Updated, TempData, 
                () => View(), 
                () => View(viewModel));
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(AccommodationController))
                .WithMethod(nameof(Update))
                .WithOperation(nameof(HttpPostAttribute))
                .WithParameter(typeof(UpdateAccommodationViewModel).FullName ?? String.Empty, nameof(viewModel), viewModel.SerializeObject())
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }

    private async Task<IEnumerable<AccommodationDomain>> GetAccommodationsWithLimits(int pageId, int countOfElements, CancellationToken cancellationToken)
    {
        return await _accommodationService.GetWithLimitsAsync(pageId * countOfElements, countOfElements, cancellationToken);
    }
}