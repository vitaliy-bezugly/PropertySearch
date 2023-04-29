using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertySearchApp.Domain;
using PropertySearchApp.Services.Abstract;
using PropertySearchApp.Extensions;
using PropertySearchApp.Common.Constants;
using PropertySearchApp.Controllers.Extensions;
using PropertySearchApp.Common.Extensions;

namespace PropertySearchApp.Controllers;

[Authorize, Route("[controller]")]
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

    [HttpGet(nameof(Create))]
    public async Task<IActionResult> Create([FromQuery] string type, [FromQuery] string content)
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
    [HttpGet((nameof(Delete)) + "/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
        {
            ModelState.AddModelError(nameof(id), "Contact id can not be empty");
            return ValidationProblem(ModelState);
        }

        Guid userId = _contextAccessor.GetUserId();
        var result = await _contactsService.DeleteContactFromUserAsync(userId, id);

        return result.ToResponse("Successfully deleted contact", TempData, 
            () => RedirectToAction("Edit", "Identity"), 
            () => RedirectToAction("Edit", "Identity"));
    }

    private bool ValidateContactIfInvalidAddErrorsToModelState(string contactType, string contactContent)
    {
        bool isFaulted = false;
        if (string.IsNullOrEmpty(contactType))
        {
            ModelState.AddModelError("type", ErrorMessages.Contacts.TypeIsEmpty);
            isFaulted = true;
        }
        if (string.IsNullOrEmpty(contactContent))
        {
            ModelState.AddModelError("content", ErrorMessages.Contacts.ContentIsEmpty);
            isFaulted = true;
        }

        return isFaulted;
    }
}
