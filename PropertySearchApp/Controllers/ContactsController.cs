using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertySearchApp.Common.Exceptions.Abstract;
using PropertySearchApp.Common.Exceptions;
using PropertySearchApp.Domain;
using PropertySearchApp.Services.Abstract;
using System.Security.Claims;
using System.Text;
using PropertySearchApp.Extensions;
using LanguageExt.Common;

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
        
        return ToResponse(result, "Successfully created new contact!");
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest();

        var userId = Guid.Parse(_contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var result = await _contactsService.DeleteContactFromUserAsync(userId, id);

        return ToResponse(result, "Successfully deleted contact!");
    }

    private IActionResult ToResponse(Result<bool> result, string successMessage)
    {
        return result.Match<IActionResult>(success =>
        {
            TempData["alert-success"] = successMessage;
            return RedirectToAction("Edit", "Identity");
        }, exception =>
        {
            if (exception is BaseApplicationException appException)
            {
                TempData["alert-danger"] = appException.BuildExceptionMessage();
                return RedirectToAction("Edit", "Identity");
            }
            else if (exception is InternalDatabaseException dbException)
            {
                TempData["alert-danger"] = "Operation failed. Try again later";
                foreach (var error in dbException.Errors)
                {
                    _logger.LogWarning(exception, error);
                }
                return RedirectToAction("Edit", "Identity");
            }

            _logger.LogError(exception, "Unhandled exception in create accommodation operation");
            throw exception;
        });
    }
}
