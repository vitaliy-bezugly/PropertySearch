using AutoMapper;
using PropertySearchApp.Common;
using PropertySearchApp.Common.Constants;
using PropertySearchApp.Common.Extensions;
using PropertySearchApp.Common.Logging;
using PropertySearchApp.Domain;
using PropertySearchApp.Entities;
using PropertySearchApp.Repositories.Abstract;
using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Services;

public class ContactService : IContactService
{
    private readonly ILogger<ContactService> _logger;
    private readonly IContactsRepository _contactsRepository;
    private readonly IUserReceiverRepository _userReceiverRepository;
    private readonly IMapper _mapper;
    public ContactService(IUserReceiverRepository userReceiverRepository, IContactsRepository contactsRepository, IMapper mapper, ILogger<ContactService> logger)
    {
        _userReceiverRepository = userReceiverRepository;
        _contactsRepository = contactsRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<ContactDomain>> GetUserContactsAsync(Guid userId)
    {
        try
        {
            var contacts = await _contactsRepository.GetUserContactsAsync(userId);
            if(contacts is null)
            {
                return new List<ContactDomain>();
            }

            return contacts.Select(x => _mapper.Map<ContactDomain>(x)).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(ContactService))
                .WithMethod(nameof(GetUserContactsAsync))
                .WithOperation("Get")
                .WithComment(e.Message)
                .WithParameter(typeof(Guid).Name, nameof(userId), userId.ToString())
                .ToString());
            
            throw;
        }
    }
    
    public async Task<OperationResult> AddContactToUserAsync(Guid userId, ContactDomain contact)
    {
        try
        {
            var user = await _userReceiverRepository.GetByIdWithContactsAsync(userId);
            if(user == null)
            {
                return new OperationResult(ErrorMessages.User.NotFound);
            }
            else if(user.Contacts != null && user.Contacts.Any(x => x.Content == contact.Content))
            {
                return new OperationResult(ErrorMessages.Contacts.AlreadyExist);
            }

            return await _contactsRepository.AddContactToUserAsync(userId, _mapper.Map<ContactEntity>(contact));
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(ContactService))
                .WithMethod(nameof(AddContactToUserAsync))
                .WithOperation("Post")
                .WithComment(e.Message)
                .WithParameter(typeof(Guid).Name, nameof(userId), userId.ToString())
                .ToString());
            
            throw;
        }
    }
    
    public async Task<OperationResult> DeleteContactFromUserAsync(Guid userId, Guid contactId)
    {
        try
        {
            var user = await _userReceiverRepository.GetByIdWithContactsAsync(userId);
            if (user == null)
            {
                return new OperationResult(ErrorMessages.User.NotFound);
            }

            return await _contactsRepository.DeleteContactAsync(contactId);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(ContactService))
                .WithMethod(nameof(DeleteContactFromUserAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(typeof(Guid).Name, nameof(userId), userId.ToString())
                .WithParameter(typeof(Guid).Name, nameof(contactId), contactId.ToString())
                .ToString());
            
            throw;
        }
    }

    public async Task<OperationResult> UpdateUserContactAsync(Guid userId, ContactDomain contact)
    {
        try
        {
            var user = await _userReceiverRepository.GetByIdWithContactsAsync(userId);
            if (user == null)
            {
                return new OperationResult(ErrorMessages.User.NotFound);
            }

            if (user.Contacts != null && user.Contacts.Any(x => x.Id == contact.Id))
            {
                return await _contactsRepository.UpdateContactAsync(_mapper.Map<ContactEntity>(contact));
            }
            else
            {
                return new OperationResult(ErrorMessages.Contacts.Forbidden);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(ContactService))
                .WithMethod(nameof(UpdateUserContactAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(nameof(Guid), nameof(userId), userId.ToString())
                .WithParameter(typeof(ContactDomain).FullName ?? String.Empty, nameof(contact), contact.SerializeObject())
                .ToString());
            
            throw;
        }
    }
}
