using PropertySearchApp.Common;
using PropertySearchApp.Entities;

namespace PropertySearchApp.Repositories.Abstract;

public interface IContactsRepository
{
    Task<List<ContactEntity>> GetUserContactsAsync(Guid userId);
    Task<OperationResult> AddContactToUserAsync(Guid userId, ContactEntity contact);
    Task<OperationResult> UpdateContactAsync(ContactEntity contact);
    Task<OperationResult> DeleteContactAsync(Guid contactId);
}
