﻿using Microsoft.EntityFrameworkCore;
using PropertySearch.Api.Common;
using PropertySearch.Api.Common.Constants;
using PropertySearch.Api.Common.Exceptions;
using PropertySearch.Api.Common.Logging;
using PropertySearch.Api.Entities;
using PropertySearch.Api.Persistence;
using PropertySearch.Api.Repositories.Abstract;
using PropertySearch.Api.Common.Extensions;

namespace PropertySearch.Api.Repositories;

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
        try
        {
            if(contact == null) throw new ArgumentNullException(nameof(contact));

            contact.UserId = userId;
            await _context.Contacts.AddAsync(contact);

            var result = await _context.SaveChangesAsync();
            if(result > 0)
                return new OperationResult();

            return GenerateInternalDatabaseException("Can not add contact to user, initial database error");
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(ContactsRepository))
                .WithMethod(nameof(AddContactToUserAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(nameof(Guid), nameof(userId), userId.ToString())
                .WithParameter(typeof(ContactEntity).FullName ?? String.Empty, nameof(contact), contact.SerializeObject())
                .ToString());
            
            throw;
        }
    }

    public async Task<OperationResult> DeleteContactAsync(Guid contactId)
    {
        try
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
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(ContactsRepository))
                .WithMethod(nameof(DeleteContactAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(nameof(Guid), nameof(contactId), contactId.ToString())
                .ToString());
            
            throw;
        }
    }

    public async Task<List<ContactEntity>> GetUserContactsAsync(Guid userId)
    {
        try
        {
            return await _context.Contacts.Where(x => x.UserId == userId).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(ContactsRepository))
                .WithMethod(nameof(GetUserContactsAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(nameof(Guid), nameof(userId), userId.ToString())
                .ToString());
            
            throw;
        }
    }

    public async Task<OperationResult> UpdateContactAsync(ContactEntity contact)
    {
        try
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
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(ContactsRepository))
                .WithMethod(nameof(UpdateContactAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(typeof(ContactEntity).FullName ?? String.Empty, nameof(contact), contact.SerializeObject())
                .ToString());

            throw;
        }
    }

    private OperationResult GenerateInternalDatabaseException(string errorMessage)
    {
        var exception = new InternalDatabaseException(new[] { errorMessage });
        _logger.LogWarning(exception, errorMessage);
        return new OperationResult(errorMessage);
    }
}