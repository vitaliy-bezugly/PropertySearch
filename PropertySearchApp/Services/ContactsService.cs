using AutoMapper;
using LanguageExt.Common;
using PropertySearchApp.Common.Exceptions;
using PropertySearchApp.Domain;
using PropertySearchApp.Entities;
using PropertySearchApp.Repositories.Abstract;
using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Services;

public class ContactsService : IContactsService
{
    private readonly IContactsRepository _contactsRepository;
    private readonly IUserReceiverRepository _userReceiverRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ContactsService> _logger;
    public ContactsService(IUserReceiverRepository userReceiverRepository, IContactsRepository contactsRepository, IMapper mapper, ILogger<ContactsService> logger)
    {
        _userReceiverRepository = userReceiverRepository;
        _contactsRepository = contactsRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<bool>> AddContactToUserAsync(Guid userId, ContactDomain contact)
    {
        var user = await _userReceiverRepository.GetByIdWithContactsAsync(userId);
        if(user == null)
        {
            return new Result<bool>(new UserNotFoundException(new string[] { "User with given id does not exist" }));
        }
        else if(user.Contacts.Any(x => x.Content == contact.Content))
        {
            return new Result<bool>(new ContactValidationException("Given user already has this contact"));
        }

        return await _contactsRepository.AddContactToUserAsync(userId, _mapper.Map<ContactEntity>(contact));
    }

    public async Task<Result<bool>> DeleteContactFromUserAsync(Guid userId, ContactDomain contact)
    {
        var user = await _userReceiverRepository.GetByIdWithContactsAsync(userId);
        if (user == null)
        {
            return new Result<bool>(new UserNotFoundException(new string[] { "User with given id does not exist" }));
        }

        return await _contactsRepository.DeleteContactAsync(_mapper.Map<ContactEntity>(contact));
    }

    public async Task<List<ContactDomain>> GetUserContactsAsync(Guid userId)
    {
        var user = await _userReceiverRepository.GetByIdWithContactsAsync(userId);
        if (user == null)
        {
            var exception = new UserNotFoundException(new string[] { "User with given id does not exist" });
            _logger.LogCritical(exception, "Can not get contacts of user that does not exist");
            throw exception;
        }

        return user.Contacts.Select(x => _mapper.Map<ContactDomain>(x)).ToList();
    }

    public async Task<Result<bool>> UpdateUserContactAsync(Guid userId, ContactDomain contact)
    {
        var user = await _userReceiverRepository.GetByIdWithContactsAsync(userId);
        if (user == null)
        {
            return new Result<bool>(new UserNotFoundException(new string[] { "User with given id does not exist" }));
        }
        else if (user.Contacts.Any(x => x.Id == contact.Id))
        {
            return await _contactsRepository.UpdateContactAsync(_mapper.Map<ContactEntity>(contact));
        }
        else
        {
            return new Result<bool>(new ContactValidationException("Given user does not have access to current contact"));
        }
    }
}
