using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PropertySearchApp.Common;
using PropertySearchApp.Common.Constants;
using PropertySearchApp.Common.Extensions;
using PropertySearchApp.Common.Logging;
using PropertySearchApp.Controllers.Extensions;
using PropertySearchApp.Domain;
using PropertySearchApp.Filters;
using PropertySearchApp.Models.Contacts;
using PropertySearchApp.Models.Identities;
using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Controllers;

[ServiceFilter(typeof(LoggingFilter))]
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
        try
        {
            return View();
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityController))
                .WithMethod(nameof(Login))
                .WithOperation(nameof(HttpGetAttribute))
                .WithNoParameters()
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }
    
    [ValidateAntiForgeryToken, HttpPost, Route(ApplicationRoutes.Identity.Login)]
    public async Task<IActionResult> Login(LoginViewModel loginModel)
    {
        try
        {
            if (ModelState.IsValid == false)
                return View(loginModel);

            var result = await _identityService.LoginAsync(loginModel.Username, loginModel.Password);
            return HandleResult(result, loginModel, null);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityController))
                .WithMethod(nameof(Login))
                .WithOperation(nameof(HttpPostAttribute))
                .WithParameter(typeof(LoginViewModel).FullName, nameof(loginModel), loginModel.SerializeObject())
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }
    
    [HttpGet, Route(ApplicationRoutes.Identity.Register)]
    public IActionResult Register()
    {
        try
        {
            return View();
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityController))
                .WithMethod(nameof(Register))
                .WithOperation(nameof(HttpGetAttribute))
                .WithNoParameters()
                .WithComment(e.Message)
                .ToString());
            throw;
        }
    }
    
    [ValidateAntiForgeryToken, HttpPost, Route(ApplicationRoutes.Identity.Register)]
    public async Task<IActionResult> Register(RegistrationFormViewModel registrationModel)
    {
        try
        {
            if (ModelState.IsValid == false)
                return View(registrationModel);

            var result = await _identityService.RegisterAsync(_mapper.Map<UserDomain>(registrationModel));
            return HandleResult(result, registrationModel, "You have successfully created an account! Check your email to confirm.");
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityController))
                .WithMethod(nameof(Register))
                .WithOperation(nameof(HttpPostAttribute))
                .WithParameter(typeof(RegistrationFormViewModel).FullName, nameof(registrationModel), registrationModel.SerializeObject())
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }
    
    [HttpPost, Authorize, Route(ApplicationRoutes.Identity.Logout)]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await _identityService.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityController))
                .WithMethod(nameof(Logout))
                .WithOperation(nameof(HttpPostAttribute))
                .WithNoParameters()
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }
    
    [HttpGet, Authorize, Route(ApplicationRoutes.Identity.Details)]
    public async Task<IActionResult> Details([FromRoute] Guid id)
    {
        try
        {
            var user = await _identityService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            var viewModel = _mapper.Map<UserDetailsViewModel>(user);
            viewModel.Contacts = (await _contactsService.GetUserContactsAsync(id)).Select(x => _mapper.Map<ContactViewModel>(x)).ToList();
            return View(viewModel);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityController))
                .WithMethod(nameof(Details))
                .WithOperation(nameof(HttpGetAttribute))
                .WithParameter(typeof(Guid).FullName, nameof(id), id.ToString())
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }
    
    [HttpGet, Authorize, Route(ApplicationRoutes.Identity.Edit)]
    public async Task<IActionResult> Edit()
    {
        try
        {
            Guid currentUserId = _httpContextAccessor.GetUserId();
            var user = await _identityService.GetUserByIdAsync(currentUserId);
        
            var request = _mapper.Map<EditUserFieldsRequest>(user);
        
            request.Contacts = (await _contactsService.GetUserContactsAsync(currentUserId)).Select(x => _mapper.Map<ContactViewModel>(x)).ToList();
        
            return user == null ? Forbid() : View(request);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityController))
                .WithMethod(nameof(Edit))
                .WithOperation(nameof(HttpGetAttribute))
                .WithNoParameters()
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }
    
    [HttpPost, ValidateAntiForgeryToken, Authorize, Route(ApplicationRoutes.Identity.Edit)]
    public async Task<IActionResult> Edit(EditUserFieldsRequest request)
    {
        try
        {
            if (ModelState.IsValid == false)
                return View(request);

            Guid userId = _httpContextAccessor.GetUserId();

            var result = await _identityService.UpdateUserFieldsAsync(userId, request.UserName, request.Information, request.PasswordToCompare);
            return result.ToResponse("Profile was updated successfully!", TempData, 
                () => RedirectToAction(nameof(Edit), "Identity"), 
                () => View(request));
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityController))
                .WithMethod(nameof(Edit))
                .WithOperation(nameof(HttpPostAttribute))
                .WithParameter(typeof(EditUserFieldsRequest).FullName, nameof(request), request.SerializeObject())
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }

    [HttpGet, Authorize, Route(ApplicationRoutes.Identity.ChangePassword)]
    public IActionResult ChangePassword()
    {
        try
        {
            var viewModel = new ChangePasswordViewModel();
            return View(viewModel);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityController))
                .WithMethod(nameof(ChangePassword))
                .WithOperation(nameof(HttpGetAttribute))
                .WithNoParameters()
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }
    
    [HttpPost, Authorize, ValidateAntiForgeryToken, Route(ApplicationRoutes.Identity.ChangePassword)]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
    {
        try
        {
            if(ModelState.IsValid == false)
                return View(viewModel);

            Guid userId = _httpContextAccessor.GetUserId();

            var result = await _identityService.ChangePasswordAsync(userId, viewModel.CurrentPassword, viewModel.NewPassword);
            return result.ToResponse("Password has been changed successfully!", TempData, 
                () => RedirectToAction(nameof(ChangePassword), "Identity"), 
                () => View(viewModel));
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityController))
                .WithMethod(nameof(ChangePassword))
                .WithOperation(nameof(HttpPostAttribute))
                .WithParameter(typeof(ChangePasswordViewModel).FullName, nameof(viewModel), viewModel.SerializeObject())
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }

    [HttpGet, AllowAnonymous, Route(ApplicationRoutes.Identity.ConfirmEmail)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailQuery query)
    {
        try
        {
            if (ModelState.IsValid == false)
                return BadRequest();
            
            OperationResult result = await _identityService.ConfirmEmailAsync(query.UserId, query.Token);
            if (result.Succeeded)
                return View(true);
            
            _logger.LogWarning($"Email confirmation failed, reason: {result.ErrorMessage}");
            return View(false);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityController))
                .WithMethod(nameof(ConfirmEmail))
                .WithOperation(nameof(HttpPostAttribute))
                .WithParameter(typeof(ConfirmEmailQuery).FullName, nameof(query), query.SerializeObject())
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }

    private IActionResult HandleResult<T>(OperationResult result, T model, string? successMessage)
    {
        if (result.Succeeded)
        {
            if(String.IsNullOrEmpty(successMessage) == false)
                TempData[Alerts.Success] = successMessage;
            return RedirectToAction("Index", "Home");
        }
        
        AddErrorsToModelState(ModelState, result);
        return View(model);
    }
    
    private void AddErrorsToModelState(ModelStateDictionary modelState, OperationResult result)
    {
        modelState.AddModelError(string.Empty, result.ErrorMessage);
    }
}