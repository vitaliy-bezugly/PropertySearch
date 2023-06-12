using PropertySearch.Api.Common;
using PropertySearch.Api.Entities;

namespace PropertySearch.Api.Repositories.Abstract;

public interface IContactsRepository
{
    Task<List<ContactEntity>> GetUserContactsAsync(Guid userId);
    Task<OperationResult> AddContactToUserAsync(Guid userId, ContactEntity contact);
    Task<OperationResult> UpdateContactAsync(ContactEntity contact);
    Task<OperationResult> DeleteContactAsync(Guid contactId);
}
