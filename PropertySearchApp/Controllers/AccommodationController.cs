using AutoMapper;
using LanguageExt.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertySearchApp.Common.Exceptions;
using PropertySearchApp.Common.Exceptions.Abstract;
using PropertySearchApp.Domain;
using PropertySearchApp.Models;
using PropertySearchApp.Repositories;
using PropertySearchApp.Services.Abstract;
using System.Security.Claims;
using System.Text;

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

    private async Task<IEnumerable<AccommodationDomain>> GetAccommodationsWithLimits(int firstElement, int countOfElements, CancellationToken cancellationToken)
    {
        return await _accommodationService.GetWithLimitsAsync(firstElement, countOfElements, cancellationToken);
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromRoute] int id, CancellationToken cancellationToken)
    {
        var accommodations = (await GetAccommodationsWithLimits(id * 12, 12, cancellationToken))
            .Select(x => _mapper.Map<AccommodationViewModel>(x));

        return View(accommodations);
    }
    [HttpGet]
    public async Task<IActionResult> MyOffers([FromRoute] int id, CancellationToken cancellationToken)
    {
        var accommodations = (await _accommodationService.GetWithLimitsAsync(id * 12, 12, cancellationToken))
            .Where(x => x.UserId == _userId)
            .Select(x => _mapper.Map<AccommodationViewModel>(x));

        return View("Index", accommodations);
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

        var accommodation = new AccommodationDomain(Guid.NewGuid(), viewModel.Title, viewModel.Description,
            viewModel.Price, viewModel.PhotoUri, _userId);

        var result = await _accommodationService.CreateAccommodationAsync(accommodation, cancellationToken);

        return ToResponse<CreateAccommodationViewModel>(result, viewModel, "Successfully created accommodation");
    }

    [ValidateAntiForgeryToken, HttpPost]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _accommodationService.DeleteAccommodationAsync(_userId, id, cancellationToken);
        return ToDeleteAccommodationResponse<IActionResult>(result);
    }
    [HttpGet]
    public async Task<IActionResult> Update([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var accommodation = await _accommodationService.GetAccommodationByIdAsync(id, cancellationToken);
        if (accommodation == null)
            return NotFound();

        var updateViewModel = _mapper.Map<UpdateAccommodationViewModel>(accommodation);
        return View(updateViewModel);
    }
    [ValidateAntiForgeryToken, HttpPost]
    public async Task<IActionResult> Update([FromForm] UpdateAccommodationViewModel viewModel, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid == false)
            return View(viewModel);

        var accommodation = new AccommodationDomain(viewModel.Id, viewModel.Title, viewModel.Description,
            viewModel.Price, viewModel.PhotoUri, _userId);

        var result = await _accommodationService.UpdateAccommodationAsync(accommodation, cancellationToken);
        return ToResponse<UpdateAccommodationViewModel>(result, viewModel, "Successfully updated accommodation");
    }

    private IActionResult ToResponse<T>(Result<bool> result, T viewModel, string successMessage)
        where T : class
    {
        return result.Match<IActionResult>(success =>
        {
            TempData["alert-success"] = successMessage;
            return View();
        }, exception =>
        {
            if (exception is BaseApplicationException appException)
            {
                TempData["alert-danger"] = BuildExceptionMessage(appException.Errors);
                return View(viewModel);
            }
            else if (exception is InternalDatabaseException dbException)
            {
                TempData["alert-danger"] = "Operation failed. Try again later";
                _logger.LogWarning(exception, BuildExceptionMessage(dbException.Errors));
                return View(viewModel);
            }

            _logger.LogError(exception, "Unhandled exception in create accommodation operation");
            throw exception;
        });
    }
    private IActionResult ToDeleteAccommodationResponse<T>(Result<bool> result)
        where T : class
    {
        return result.Match<IActionResult>(success =>
        {
            TempData["alert-success"] = "Successfully deleted accommodation";
            return RedirectToAction("Index", "Accommodation");
        }, exception =>
        {
            if (exception is BaseApplicationException appException)
            {
                TempData["alert-danger"] = BuildExceptionMessage(appException.Errors);
                return RedirectToAction("Index", "Accommodation");
            }
            else if (exception is InternalDatabaseException dbException)
            {
                TempData["alert-danger"] = "Operation failed. Try again later";
                _logger.LogWarning(exception, BuildExceptionMessage(dbException.Errors));
                return RedirectToAction("Index", "Accommodation");
            }

            _logger.LogError(exception, "Unhandled exception in create accommodation operation");
            throw exception;
        });
    }
    private static string BuildExceptionMessage(string[] errors)
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in errors)
        {
            stringBuilder.Append(item);
        }
        return stringBuilder.ToString();
    }
}
