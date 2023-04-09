using LanguageExt.Common;
using PropertySearchApp.Entities;
using PropertySearchApp.Persistence;
using PropertySearchApp.Repositories.Abstract;

namespace PropertySearchApp.Repositories;

public class ContactsRepository : IContactsRepository
{
    private readonly ApplicationDbContext _context;
    public ContactsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> AddContactToUserAsync(Guid userId, ContactEntity contact)
    {
        if(contact == null) throw new ArgumentNullException(nameof(contact));

        await _context.Contacts.AddAsync(contact);
        return true;
    }

    public Task<Result<bool>> DeleteContactAsync(ContactEntity contact)
    {
        throw new NotImplementedException();
    }

    public Task<List<ContactEntity>> GetUserContactsAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> UpdateContactAsync(ContactEntity contact)
    {
        throw new NotImplementedException();
    }
}
