using AutoMapper;
using LanguageExt.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PropertySearchApp.Common.Exceptions;
using PropertySearchApp.Common.Exceptions.Abstract;
using PropertySearchApp.Domain;
using PropertySearchApp.Models;
using PropertySearchApp.Services.Abstract;
using System.Security.Claims;
using System.Text;

namespace PropertySearchApp.Controllers;

public class IdentityController : Controller
{
    private readonly IIdentityService _identityService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ILogger<IdentityController> _logger;
    public IdentityController(IIdentityService identityService, IMapper mapper, IHttpContextAccessor contextAccessor, ILogger<IdentityController> logger)
    {
        _identityService = identityService;
        _mapper = mapper;
        _httpContextAccessor = contextAccessor;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [ValidateAntiForgeryToken, HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginModel)
    {
        if (ModelState.IsValid == false)
            return View(loginModel);

        var result = await _identityService.LoginAsync(loginModel.Username, loginModel.Password);

        return result.Match<IActionResult>(success =>
        {
            return RedirectToAction("Index", "Home");
        }, exception =>
        {
            if (AddErrorsToModelState(ModelState, exception))
                return View(loginModel);
            else
                throw exception;
        });
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [ValidateAntiForgeryToken, HttpPost]
    public async Task<IActionResult> Register(RegistrationFormViewModel registrationModel)
    {
        if (ModelState.IsValid == false)
            return View(registrationModel);

        Result<bool> result = await _identityService.RegisterAsync(_mapper.Map<UserDomain>(registrationModel));

        return result.Match<IActionResult>(success =>
        {
            return RedirectToAction("Index", "Home");
        }, exception =>
        {
            if (AddErrorsToModelState(ModelState, exception))
                return View(registrationModel);
            else
                throw exception;
        });
    }
    [HttpPost, Authorize]
    public async Task<IActionResult> Logout()
    {
        await _identityService.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    [HttpGet, Authorize]
    public async Task<IActionResult> Details([FromRoute] Guid id)
    {
        var user = await _identityService.GetUserByIdAsync(id);
        return user == null ? NotFound() : View(_mapper.Map<UserDetailsViewModel>(user));
    }
    [HttpGet, Authorize]
    public async Task<IActionResult> Edit()
    {
        var currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var user = await _identityService.GetUserByIdAsync(currentUserId);
        var request = _mapper.Map<EditUserFieldsRequest>(user);
        return user == null ? Forbid() : View(request);
    }
    [HttpPost, ValidateAntiForgeryToken, Authorize]
    public async Task<IActionResult> Edit(EditUserFieldsRequest request)
    {
        if (ModelState.IsValid == false)
            return View(request);

        var requestToService = new EditUserFieldsRequest(request);
        /* Add validator from DI */
        if(string.IsNullOrEmpty(request.ContactToAdd.ContactType) == false && string.IsNullOrEmpty(request.ContactToAdd.Content) == false
            && request.Contacts.Any(x => x.Content == request.ContactToAdd.Content) == false)
        {
            requestToService.Contacts.Add(requestToService.ContactToAdd);
        }

        var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var user = new UserDomain
        {
            Id = userId,
            Username = requestToService.UserName,
            Information = requestToService.Information,
            Contacts = requestToService.Contacts.Select(x => _mapper.Map<ContactDomain>(x)).ToList(),
            Password = requestToService.PasswordToCompare
        };

        var result = await _identityService.UpdateUserFields(user);
        return ToResponse(result, request, "Profile was updated successfully!");
    }

    private IActionResult ToResponse<T>(Result<bool> result, T viewModel, string successMessage)
    {
        return result.Match<IActionResult>(success =>
        {
            TempData["alert-success"] = successMessage;
            return RedirectToAction(nameof(Edit), "Identity");
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
    private static string BuildExceptionMessage(string[] errors)
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in errors)
        {
            stringBuilder.Append(item);
        }
        return stringBuilder.ToString();
    }
    private bool AddErrorsToModelState(ModelStateDictionary modelState, Exception exception)
    {
        if (exception is AuthorizationOperationException registrationException)
        {
            foreach (var error in registrationException.GetErrors())
            {
                modelState.AddModelError(string.Empty, error);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}