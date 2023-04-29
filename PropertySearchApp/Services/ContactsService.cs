using AutoMapper;
using PropertySearchApp.Common;
using PropertySearchApp.Common.Constants;
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

    public async Task<OperationResult> AddContactToUserAsync(Guid userId, ContactDomain contact)
    {
        var user = await _userReceiverRepository.GetByIdWithContactsAsync(userId);
        if(user == null)
        {
            return new OperationResult(ErrorMessages.User.NotFound);
        }
        else if(user.Contacts.Any(x => x.Content == contact.Content))
        {
            return new OperationResult(ErrorMessages.Contacts.AlreadyExist);
        }

        return await _contactsRepository.AddContactToUserAsync(userId, _mapper.Map<ContactEntity>(contact));
    }

    public async Task<OperationResult> DeleteContactFromUserAsync(Guid userId, Guid contactId)
    {
        var user = await _userReceiverRepository.GetByIdWithContactsAsync(userId);
        if (user == null)
        {
            return new OperationResult(ErrorMessages.User.NotFound);
        }

        return await _contactsRepository.DeleteContactAsync(contactId);
    }

    public async Task<List<ContactDomain>> GetUserContactsAsync(Guid userId)
    {
        var contacts = await _contactsRepository.GetUserContactsAsync(userId);
        return contacts.Select(x => _mapper.Map<ContactDomain>(x)).ToList();
    }

    public async Task<OperationResult> UpdateUserContactAsync(Guid userId, ContactDomain contact)
    {
        var user = await _userReceiverRepository.GetByIdWithContactsAsync(userId);
        if (user == null)
        {
            return new OperationResult(ErrorMessages.User.NotFound);
        }

        if (user.Contacts.Any(x => x.Id == contact.Id))
        {
            return await _contactsRepository.UpdateContactAsync(_mapper.Map<ContactEntity>(contact));
        }
        else
        {
            return new OperationResult(ErrorMessages.Contacts.Forbidden);
        }
    }
}
