using PropertySearch.Api.Common;
using PropertySearch.Api.Domain;

namespace PropertySearch.Api.Services.Abstract;

public interface IContactService
{
    Task<List<ContactDomain>> GetUserContactsAsync(Guid userId);
    Task<OperationResult> AddContactToUserAsync(Guid userId, ContactDomain contact, CancellationToken cancellationToken);
    Task<OperationResult> UpdateAsync(Guid userId, ContactDomain contact, CancellationToken cancellationToken);
    Task<OperationResult> DeleteAsync(Guid userId, Guid contactId, CancellationToken cancellationToken);
}