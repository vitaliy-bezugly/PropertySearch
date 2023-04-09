using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using PropertySearchApp.Common.Exceptions;
using PropertySearchApp.Entities;
using PropertySearchApp.Persistence;
using PropertySearchApp.Repositories.Abstract;

namespace PropertySearchApp.Repositories;

public class ContactsRepository : IContactsRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ContactsRepository> _logger;
    public ContactsRepository(ApplicationDbContext context, ILogger<ContactsRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<bool>> AddContactToUserAsync(Guid userId, ContactEntity contact)
    {
        if(contact == null) throw new ArgumentNullException(nameof(contact));

        contact.UserEntityId = userId;
        await _context.Contacts.AddAsync(contact);

        var result = await _context.SaveChangesAsync();
        if(result > 0)
            return new Result<bool>(true);

        return GenerateInternalDatabaseException("Can not add contact to user, initial database error");
    }

    public async Task<Result<bool>> DeleteContactAsync(Guid contactId)
    {
        var exists = await _context.Contacts.FirstOrDefaultAsync(x => x.Id == contactId);
        if (exists == null)
            return new Result<bool>(new ContactsNotFoundException("There is no contact with given id"));

        _context.Contacts.Remove(exists);
        var result = await _context.SaveChangesAsync();
        if (result > 0)
            return new Result<bool>(true);

        return GenerateInternalDatabaseException("Can not delete contact to user, initial database error");
    }

    public async Task<List<ContactEntity>> GetUserContactsAsync(Guid userId)
    {
        return await _context.Contacts.Where(x => x.UserEntityId == userId).ToListAsync();
    }

    public async Task<Result<bool>> UpdateContactAsync(ContactEntity contact)
    {
        if (contact == null) throw new ArgumentNullException(nameof(contact));

        var toUpdate = await _context.Contacts.FirstOrDefaultAsync(x => x.Id == contact.Id);
        if(toUpdate == null)
            return new Result<bool>(new ContactsNotFoundException("There is no contact with given id"));

        toUpdate.ContactType = contact.ContactType;
        toUpdate.Content = contact.Content; 

        var result = await _context.SaveChangesAsync();
        if (result > 0)
            return new Result<bool>(true);

        return GenerateInternalDatabaseException("Can not update contact, initial database error");
    }

    private Result<bool> GenerateInternalDatabaseException(string errorMessage)
    {
        var exception = new InternalDatabaseException(new string[] { errorMessage });
        _logger.LogWarning(exception, errorMessage);
        return new Result<bool>(exception);
    }
}
