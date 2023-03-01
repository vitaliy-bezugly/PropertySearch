using LanguageExt.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertySearchApp.Common.Exceptions;
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

        Result<bool> result = await _userService.RegisterAsync(registrationModel.Username, registrationModel.Email, registrationModel.Password);

        return result.Match<IActionResult>(success => RedirectToAction("Index", "Home"), 
            exception =>
        {
            if (exception is RegistrationOperationException registrationException)
            {
                foreach (var error in registrationException.GetErrors())
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return View(registrationModel);
            }
            else
            {
                throw exception;
            }
        });
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> Logout()
    {
        await _userService.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}