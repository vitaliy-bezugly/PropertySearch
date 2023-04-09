using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertySearchApp.Domain;
using PropertySearchApp.Services.Abstract;
using System.Security.Claims;
using PropertySearchApp.Extensions;

namespace PropertySearchApp.Controllers;

[Authorize]
public class ContactsController : Controller
{
    private readonly IContactsService _contactsService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILogger<ContactsController> _logger;
    public ContactsController(IContactsService contactsService, IHttpContextAccessor contextAccessor, ILogger<ContactsController> logger)
    {
        _contactsService = contactsService;
        _contextAccessor = contextAccessor;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Create([FromQuery] string type, [FromQuery] string content)
    {
        if (string.IsNullOrWhiteSpace(content) || string.IsNullOrEmpty(type))
            return BadRequest();

        var contact = new ContactDomain { Id = Guid.NewGuid(), ContactType = type, Content = content };
        var userId = Guid.Parse(_contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var result = await _contactsService.AddContactToUserAsync(userId, contact);

        return result.ToResponse("Successfully created contact", TempData, () => RedirectToAction("Edit", "Identity"), () => RedirectToAction("Edit", "Identity"), (exception, message) => _logger.LogError(exception, message));
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest();

        var userId = Guid.Parse(_contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var result = await _contactsService.DeleteContactFromUserAsync(userId, id);

        return result.ToResponse("Successfully deleted contact", TempData, () => RedirectToAction("Edit", "Identity"), () => RedirectToAction("Edit", "Identity"), (exception, message) => _logger.LogError(exception, message));
    }
}
