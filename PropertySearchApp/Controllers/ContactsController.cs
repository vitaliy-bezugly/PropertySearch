using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertySearchApp.Common.Exceptions.Abstract;
using PropertySearchApp.Common.Exceptions;
using PropertySearchApp.Domain;
using PropertySearchApp.Models;
using PropertySearchApp.Services.Abstract;
using System.Security.Claims;
using System.Text;

namespace PropertySearchApp.Controllers;

[Authorize]
public class ContactsController : Controller
{
    private readonly IContactsService _contactsService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILogger _logger;
    public ContactsController(IContactsService contactsService, IHttpContextAccessor contextAccessor, ILogger logger)
    {
        _contactsService = contactsService;
        _contextAccessor = contextAccessor;
        _logger = logger;
    }

    public async Task<IActionResult> AddContact([FromRoute] string contactType, [FromRoute] string content)
    {
        var contact = new ContactDomain { Id = Guid.NewGuid(), ContactType = contactType, Content = content };
        var userId = Guid.Parse(_contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var result = await _contactsService.AddContactToUserAsync(userId, contact);

        return result.Match<IActionResult>(success =>
        {
            TempData["alert-success"] = "Successfully added new contact!";
            return RedirectToAction("Edit", "Identity");
        }, exception =>
        {
            if (exception is BaseApplicationException appException)
            {
                TempData["alert-danger"] = BuildExceptionMessage(appException.Errors);
                return RedirectToAction("Edit", "Identity");
            }
            else if (exception is InternalDatabaseException dbException)
            {
                TempData["alert-danger"] = "Operation failed. Try again later";
                _logger.LogWarning(exception, BuildExceptionMessage(dbException.Errors));
                return RedirectToAction("Edit", "Identity");
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
}
