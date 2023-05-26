using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertySearchApp.Domain;
using PropertySearchApp.Services.Abstract;
using PropertySearchApp.Common.Constants;
using PropertySearchApp.Controllers.Extensions;
using PropertySearchApp.Common.Extensions;
using PropertySearchApp.Common.Logging;
using PropertySearchApp.Filters;

namespace PropertySearchApp.Controllers;

[Authorize]
[ServiceFilter(typeof(LoggingFilter))]
public class ContactController : Controller
{
    private readonly IContactService _contactsService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILogger<ContactController> _logger;
    public ContactController(IContactService contactsService, IHttpContextAccessor contextAccessor, ILogger<ContactController> logger)
    {
        _contactsService = contactsService;
        _contextAccessor = contextAccessor;
        _logger = logger;
    }

    [HttpGet, Route(ApplicationRoutes.Contact.Create)]
    public async Task<IActionResult> Create([FromQuery] string type, [FromQuery] string content)
    {
        try
        {
            bool isFaulted = ValidateContactIfInvalidAddErrorsToModelState(type, content);
            if (isFaulted)
                return ValidationProblem(ModelState);

            var contact = new ContactDomain { Id = Guid.NewGuid(), ContactType = type, Content = content };
            Guid userId = _contextAccessor.GetUserId();
            var result = await _contactsService.AddContactToUserAsync(userId, contact);

            return result.ToResponse(SuccessMessages.Contacts.Created, TempData, 
                () => RedirectToAction("Edit", "Identity"), 
                () => RedirectToAction("Edit", "Identity"));
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(ContactController))
                .WithMethod(nameof(Create))
                .WithOperation(nameof(HttpGetAttribute))
                .WithParameter(typeof(String).FullName, nameof(type), type)
                .WithParameter(typeof(String).FullName, nameof(content), content)
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }
    [HttpGet, Route(ApplicationRoutes.Contact.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                ModelState.AddModelError(nameof(id), ErrorMessages.Contacts.Validation.IdIsEmpty);
                return ValidationProblem(ModelState);
            }

            Guid userId = _contextAccessor.GetUserId();
            var result = await _contactsService.DeleteContactFromUserAsync(userId, id);

            return result.ToResponse(SuccessMessages.Contacts.Deleted, TempData, 
                () => RedirectToAction("Edit", "Identity"), 
                () => RedirectToAction("Edit", "Identity"));
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(ContactController))
                .WithMethod(nameof(Delete))
                .WithOperation(nameof(HttpGetAttribute))
                .WithParameter(typeof(Guid).FullName, nameof(id), id.ToString())
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }

    private bool ValidateContactIfInvalidAddErrorsToModelState(string contactType, string contactContent)
    {
        bool isFaulted = false;
        if (string.IsNullOrEmpty(contactType))
        {
            ModelState.AddModelError("type", ErrorMessages.Contacts.Validation.TypeIsEmpty);
            isFaulted = true;
        }
        if (string.IsNullOrEmpty(contactContent))
        {
            ModelState.AddModelError("content", ErrorMessages.Contacts.Validation.ContentIsEmpty);
            isFaulted = true;
        }

        return isFaulted;
    }
}
