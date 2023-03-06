using LanguageExt.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PropertySearchApp.Common.Exceptions;
using PropertySearchApp.Common.Exceptions.Abstract;
using PropertySearchApp.Domain;
using PropertySearchApp.Models;
using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Controllers;

public class IdentityController : Controller
{
    private readonly IUserService _userService;
    public IdentityController(IUserService userService)
    {
        _userService = userService;
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

        var result = await _userService.LoginAsync(loginModel.Email, loginModel.Password);

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

        Result<bool> result = await _userService.RegisterAsync(new UserDomain(registrationModel.Username, registrationModel.Email, registrationModel.Password, registrationModel.IsLandlord));

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
        await _userService.SignOutAsync();
        return RedirectToAction("Index", "Home");
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