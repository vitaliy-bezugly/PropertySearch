using Microsoft.AspNetCore.Mvc;

namespace PropertySearchApp.Controllers;

public class IdentityController : Controller
{
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
    [HttpPost]
    public IActionResult Logout()
    {
        throw new NotImplementedException();
    }
}