using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PropertySearchApp.Common;
using PropertySearchApp.Common.Constants;
using PropertySearchApp.Common.Extensions;
using PropertySearchApp.Controllers.Extensions;
using PropertySearchApp.Domain;
using PropertySearchApp.Extensions;
using PropertySearchApp.Models.Contacts;
using PropertySearchApp.Models.Identities;
using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Controllers;

public class IdentityController : Controller
{
    private readonly IIdentityService _identityService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ILogger<IdentityController> _logger;
    private readonly IContactService _contactsService;
    public IdentityController(IIdentityService identityService, IMapper mapper, IHttpContextAccessor contextAccessor, ILogger<IdentityController> logger, IContactService contactsService)
    {
        _identityService = identityService;
        _mapper = mapper;
        _httpContextAccessor = contextAccessor;
        _logger = logger;
        _contactsService = contactsService;
    }

    [HttpGet, Route(ApplicationRoutes.Identity.Login)]
    public IActionResult Login()
    {
        return View();
    }
    [ValidateAntiForgeryToken, HttpPost, Route(ApplicationRoutes.Identity.Login)]
    public async Task<IActionResult> Login(LoginViewModel loginModel)
    {
        if (ModelState.IsValid == false)
            return View(loginModel);

        var result = await _identityService.LoginAsync(loginModel.Username, loginModel.Password);
        return HandleResult(result, loginModel);
    }
    [HttpGet, Route(ApplicationRoutes.Identity.Register)]
    public IActionResult Register()
    {
        return View();
    }
    [ValidateAntiForgeryToken, HttpPost, Route(ApplicationRoutes.Identity.Register)]
    public async Task<IActionResult> Register(RegistrationFormViewModel registrationModel)
    {
        if (ModelState.IsValid == false)
            return View(registrationModel);

        var result = await _identityService.RegisterAsync(_mapper.Map<UserDomain>(registrationModel));
        return HandleResult(result, registrationModel);
    }
    [HttpPost, Authorize, Route(ApplicationRoutes.Identity.Logout)]
    public async Task<IActionResult> Logout()
    {
        await _identityService.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    [HttpGet, Authorize, Route(ApplicationRoutes.Identity.Details)]
    public async Task<IActionResult> Details([FromRoute] Guid id)
    {
        var user = await _identityService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();

        var viewModel = _mapper.Map<UserDetailsViewModel>(user);
        viewModel.Contacts = (await _contactsService.GetUserContactsAsync(id)).Select(x => _mapper.Map<ContactViewModel>(x)).ToList();
        return View(viewModel);
    }
    [HttpGet, Authorize, Route(ApplicationRoutes.Identity.Edit)]
    public async Task<IActionResult> Edit()
    {
        Guid currentUserId = _httpContextAccessor.GetUserId();
        var user = await _identityService.GetUserByIdAsync(currentUserId);
        
        var request = _mapper.Map<EditUserFieldsRequest>(user);
        
        request.Contacts = (await _contactsService.GetUserContactsAsync(currentUserId)).Select(x => _mapper.Map<ContactViewModel>(x)).ToList();
        
        return user == null ? Forbid() : View(request);
    }
    [HttpPost, ValidateAntiForgeryToken, Authorize, Route(ApplicationRoutes.Identity.Edit)]
    public async Task<IActionResult> Edit(EditUserFieldsRequest request)
    {
        if (ModelState.IsValid == false)
            return View(request);

        Guid userId = _httpContextAccessor.GetUserId();

        var result = await _identityService.UpdateUserFieldsAsync(userId, request.UserName, request.Information, request.PasswordToCompare);
        return result.ToResponse("Profile was updated successfully!", TempData, 
            () => RedirectToAction(nameof(Edit), "Identity"), 
            () => View(request));
    }

    [HttpGet, Authorize, Route(ApplicationRoutes.Identity.ChangePassword)]
    public IActionResult ChangePassword()
    {
        var viewModel = new ChangePasswordViewModel();
        return View(viewModel);
    }
    [HttpPost, Authorize, ValidateAntiForgeryToken, Route(ApplicationRoutes.Identity.ChangePassword)]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
    {
        if(ModelState.IsValid == false)
            return View(viewModel);

        Guid userId = _httpContextAccessor.GetUserId();

        var result = await _identityService.ChangePasswordAsync(userId, viewModel.CurrentPassword, viewModel.NewPassword);
        return result.ToResponse("Password has been changed successfully!", TempData, 
            () => RedirectToAction(nameof(ChangePassword), "Identity"), 
            () => View(viewModel));
    }

    private IActionResult HandleResult<T>(OperationResult result, T model)
    {
        if (result.Succeeded)
            return RedirectToAction("Index", "Home");

        AddErrorsToModelState(ModelState, result);
        return View(model);
    }
    private void AddErrorsToModelState(ModelStateDictionary modelState, OperationResult result)
    {
        modelState.AddModelError(string.Empty, result.ErrorMessage);
    }
}