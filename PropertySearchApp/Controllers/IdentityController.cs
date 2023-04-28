using AutoMapper;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PropertySearchApp.Common.Exceptions.Abstract;
using PropertySearchApp.Domain;
using PropertySearchApp.Extensions;
using PropertySearchApp.Models;
using PropertySearchApp.Services.Abstract;
using System.Security.Claims;

namespace PropertySearchApp.Controllers;

public class IdentityController : Controller
{
    private readonly IIdentityService _identityService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ILogger<IdentityController> _logger;
    private readonly IContactsService _contactsService;
    public IdentityController(IIdentityService identityService, IMapper mapper, IHttpContextAccessor contextAccessor, ILogger<IdentityController> logger, IContactsService contactsService)
    {
        _identityService = identityService;
        _mapper = mapper;
        _httpContextAccessor = contextAccessor;
        _logger = logger;
        _contactsService = contactsService;
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
        if (user == null)
            return NotFound();

        var viewModel = _mapper.Map<UserDetailsViewModel>(user);
        viewModel.Contacts = (await _contactsService.GetUserContactsAsync(id)).Select(x => _mapper.Map<ContactViewModel>(x)).ToList();
        
        return View(viewModel);
    }
    [HttpGet, Authorize]
    public async Task<IActionResult> Edit()
    {
        var currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var user = await _identityService.GetUserByIdAsync(currentUserId);
        
        var request = _mapper.Map<EditUserFieldsRequest>(user);
        
        request.Contacts = (await _contactsService.GetUserContactsAsync(currentUserId)).Select(x => _mapper.Map<ContactViewModel>(x)).ToList();
        
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

        var result = await _identityService.UpdateUserFields(userId, requestToService.UserName, requestToService.Information, requestToService.PasswordToCompare);
        return result.ToResponse("Profile was updated successfully!", TempData, () => RedirectToAction(nameof(Edit), "Identity"), () => View(request), (exception, message) => _logger.LogError(exception, message));
    }

    [HttpGet, Authorize]
    public IActionResult ChangePassword()
    {
        var viewModel = new ChangePasswordViewModel();
        return View(viewModel);
    }

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
    {
        throw new NotImplementedException();
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