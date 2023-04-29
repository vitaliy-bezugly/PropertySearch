using PropertySearchApp.Common;
using PropertySearchApp.Domain;

namespace PropertySearchApp.Services.Abstract;

public interface IContactsService
{
    Task<List<ContactDomain>> GetUserContactsAsync(Guid userId);
    Task<OperationResult> AddContactToUserAsync(Guid userId, ContactDomain contact);
    Task<OperationResult> UpdateUserContactAsync(Guid userId, ContactDomain contact);
    Task<OperationResult> DeleteContactFromUserAsync(Guid userId, Guid contactId);
}