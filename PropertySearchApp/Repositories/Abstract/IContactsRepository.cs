using LanguageExt.Common;
using PropertySearchApp.Entities;

namespace PropertySearchApp.Repositories.Abstract;

public interface IContactsRepository
{
    Task<List<ContactEntity>> GetUserContactsAsync(Guid userId);
    Task<Result<bool>> AddContactToUserAsync(Guid userId, ContactEntity contact);
    Task<Result<bool>> UpdateContactAsync(ContactEntity contact);
    Task<Result<bool>> DeleteContactAsync(Guid contactId);
}
