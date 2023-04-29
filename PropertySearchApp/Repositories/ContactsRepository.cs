using Microsoft.EntityFrameworkCore;
using PropertySearchApp.Common;
using PropertySearchApp.Common.Constants;
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

    public async Task<OperationResult> AddContactToUserAsync(Guid userId, ContactEntity contact)
    {
        if(contact == null) throw new ArgumentNullException(nameof(contact));

        contact.UserId = userId;
        await _context.Contacts.AddAsync(contact);

        var result = await _context.SaveChangesAsync();
        if(result > 0)
            return new OperationResult();

        return GenerateInternalDatabaseException("Can not add contact to user, initial database error");
    }

    public async Task<OperationResult> DeleteContactAsync(Guid contactId)
    {
        var exists = await _context.Contacts.FirstOrDefaultAsync(x => x.Id == contactId);
        if (exists == null)
            return new OperationResult(ErrorMessages.Contacts.NotFound);

        _context.Contacts.Remove(exists);
        var result = await _context.SaveChangesAsync();
        if (result > 0)
            return new OperationResult();

        return GenerateInternalDatabaseException("Can not delete contact to user, initial database error");
    }

    public async Task<List<ContactEntity>> GetUserContactsAsync(Guid userId)
    {
        return await _context.Contacts.Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<OperationResult> UpdateContactAsync(ContactEntity contact)
    {
        if (contact == null) throw new ArgumentNullException(nameof(contact));

        var toUpdate = await _context.Contacts.FirstOrDefaultAsync(x => x.Id == contact.Id);
        if(toUpdate == null)
            return new OperationResult(ErrorMessages.Contacts.NotFound);

        toUpdate.ContactType = contact.ContactType;
        toUpdate.Content = contact.Content; 

        var result = await _context.SaveChangesAsync();
        if (result > 0)
            return new OperationResult();

        return GenerateInternalDatabaseException("Can not update contact, initial database error");
    }

    private OperationResult GenerateInternalDatabaseException(string errorMessage)
    {
        var exception = new InternalDatabaseException(new string[] { errorMessage });
        _logger.LogWarning(exception, errorMessage);
        return new OperationResult(errorMessage);
    }
}
