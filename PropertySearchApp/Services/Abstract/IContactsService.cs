using LanguageExt.Common;
using PropertySearchApp.Domain;

namespace PropertySearchApp.Services.Abstract;

public interface IContactsService
{
    Task<List<ContactDomain>> GetUserContactsAsync(Guid userId);
    Task<Result<bool>> AddContactToUserAsync(Guid userId, ContactDomain contact);
    Task<Result<bool>> UpdateUserContactAsync(Guid userId, ContactDomain contact);
    Task<Result<bool>> DeleteContactFromUserAsync(Guid userId, Guid contactId);
}