using AutoMapper;
using LanguageExt.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
    public async Task<IActionResult> Index([FromRoute]int id, CancellationToken cancellationToken)
    {
        var accommodations = (await _accommodationService.GetWithLimitsAsync(id * 12, 12, cancellationToken))
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
        
        var accommodation = new AccommodationDomain(Guid.NewGuid(),viewModel.Title, viewModel.Description,
            viewModel.Price, viewModel.PhotoUri, _userId);

        var result = await _accommodationService.CreateAccommodationAsync(accommodation, cancellationToken);

        return ToCreateAccommodationResponse<CreateAccommodationViewModel>(result, viewModel);
    }

    [ValidateAntiForgeryToken, HttpPost]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _accommodationService.DeleteAccommodationAsync(_userId, id, cancellationToken);
        return ToDeleteAccommodationResponse<IActionResult>(result);
    }

    private IActionResult ToCreateAccommodationResponse<T>(Result<bool> result, T viewModel)
        where T : class
    {
        return result.Match<IActionResult>(success =>
        {
            TempData["alert-success"] = "Successfully created accommodation";
            return View();
        }, exception =>
        {
            if (exception is BaseApplicationException appException)
            {
                TempData["alert-danger"] = BuildExceptionMessage(appException.Errors);
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
