using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertySearch.Api.Common.Constants;
using PropertySearch.Api.Common.Logging;
using PropertySearch.Api.Domain;
using PropertySearch.Api.Filters;
using PropertySearch.Api.Services.Abstract;
using PropertySearch.Api.Controllers.Extensions;
using PropertySearch.Api.Common.Extensions;
using PropertySearch.Api.Models.Queries;

namespace PropertySearch.Api.Controllers;

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
    public async Task<IActionResult> Create([FromQuery] CreateContactQuery query, CancellationToken cancellationToken)
    {
        try
        {
            bool isFaulted = ValidateContactIfInvalidAddErrorsToModelState(query.Type, query.Content);
            if (isFaulted)
                return ValidationProblem(ModelState);

            var contact = new ContactDomain { Id = Guid.NewGuid(), ContactType = query.Type, Content = query.Content };
            Guid userId = _contextAccessor.GetUserId();
            var result = await _contactsService.AddContactToUserAsync(userId, contact, cancellationToken);

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
                .WithParameter(typeof(CreateContactQuery).FullName ?? string.Empty , nameof(query), query.SerializeObject())
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }
    [HttpGet, Route(ApplicationRoutes.Contact.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            if (id == Guid.Empty)
            {
                ModelState.AddModelError(nameof(id), ErrorMessages.Contacts.Validation.IdIsEmpty);
                return ValidationProblem(ModelState);
            }

            Guid userId = _contextAccessor.GetUserId();
            var result = await _contactsService.DeleteAsync(userId, id, cancellationToken);

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
                .WithParameter(typeof(Guid).FullName ?? String.Empty, nameof(id), id.ToString())
                .WithComment(e.Message)
                .ToString());
            
            throw;
        }
    }

    private bool ValidateContactIfInvalidAddErrorsToModelState(string type, string content)
    {
        bool isFaulted = false;
        if (string.IsNullOrEmpty(type))
        {
            ModelState.AddModelError("type", ErrorMessages.Contacts.Validation.TypeIsEmpty);
            isFaulted = true;
        }
        if (string.IsNullOrEmpty(content))
        {
            ModelState.AddModelError("content", ErrorMessages.Contacts.Validation.ContentIsEmpty);
            isFaulted = true;
        }

        return isFaulted;
    }
}
