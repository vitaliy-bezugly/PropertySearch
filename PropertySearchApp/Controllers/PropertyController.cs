using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PropertySearchApp.Controllers;

[Authorize]
public class PropertyController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
