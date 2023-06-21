using Microsoft.EntityFrameworkCore;
using PropertySearch.Api.Entities;
using PropertySearch.Api.Persistence;
using PropertySearch.Api.Repositories.Abstract;

namespace PropertySearch.Api.Repositories;

public class ContactsRepository : RepositoryBase<ContactEntity>, IContactsRepository
{
    private readonly ApplicationDbContext _context;
    public ContactsRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    

    public async Task<List<ContactEntity>> GetUserContactsAsync(Guid userId)
    {
        List<ContactEntity> userContacts = await _context.Contacts.Where(x => x.UserId == userId).ToListAsync();
        return userContacts;
    }
}
