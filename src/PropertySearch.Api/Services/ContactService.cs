using AutoMapper;
using PropertySearch.Api.Common;
using PropertySearch.Api.Common.Constants;
using PropertySearch.Api.Common.Logging;
using PropertySearch.Api.Domain;
using PropertySearch.Api.Entities;
using PropertySearch.Api.Repositories.Abstract;
using PropertySearch.Api.Services.Abstract;
using PropertySearch.Api.Common.Extensions;
using PropertySearch.Api.Persistence;

namespace PropertySearch.Api.Services;

public class ContactService : IContactService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ContactService> _logger;
    private readonly IUserReceiverRepository _userReceiverRepository;
    private readonly IMapper _mapper;
    
    public ContactService(IUnitOfWork unitOfWork, IUserReceiverRepository userReceiverRepository, IMapper mapper, ILogger<ContactService> logger)
    {
        _unitOfWork = unitOfWork;
        _userReceiverRepository = userReceiverRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<ContactDomain>> GetUserContactsAsync(Guid userId)
    {
        try
        {
            var contacts = await _unitOfWork.ContactsRepository.GetUserContactsAsync(userId);
            return contacts.Select(x => _mapper.Map<ContactDomain>(x)).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(ContactService))
                .WithMethod(nameof(GetUserContactsAsync))
                .WithOperation("Get")
                .WithComment(e.Message)
                .WithParameter(nameof(Guid), nameof(userId), userId.ToString())
                .ToString());
            
            throw;
        }
    }
    
    public async Task<OperationResult> AddContactToUserAsync(Guid userId, ContactDomain contact, CancellationToken cancellationToken)
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

            ContactEntity entity = _mapper.Map<ContactEntity>(contact);
            entity.UserId = userId;
            
            _unitOfWork.ContactsRepository.Insert(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Success;
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(ContactService))
                .WithMethod(nameof(AddContactToUserAsync))
                .WithOperation("Post")
                .WithComment(e.Message)
                .WithParameter(nameof(Guid), nameof(userId), userId.ToString())
                .ToString());
            
            throw;
        }
    }
    
    public async Task<OperationResult> DeleteAsync(Guid userId, Guid contactId, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userReceiverRepository.GetByIdWithContactsAsync(userId);
            if (user is null)
            {
                return new OperationResult(ErrorMessages.User.NotFound);
            }
            
            if (ValidateUserAccessToContact(contactId, user) == false)
                return new OperationResult(ErrorMessages.Contacts.Forbidden);
            
            await _unitOfWork.ContactsRepository.DeleteAsync(contactId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Success;
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(ContactService))
                .WithMethod(nameof(DeleteAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(nameof(Guid), nameof(userId), userId.ToString())
                .WithParameter(nameof(Guid), nameof(contactId), contactId.ToString())
                .ToString());
            
            throw;
        }
    }

    public async Task<OperationResult> UpdateAsync(Guid userId, ContactDomain contact, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userReceiverRepository.GetByIdWithContactsAsync(userId);
            if (user == null)
            {
                return new OperationResult(ErrorMessages.User.NotFound);
            }
            
            if (ValidateUserAccessToContact(contact.Id, user) == false)
                return new OperationResult(ErrorMessages.Contacts.Forbidden);
            
            await _unitOfWork.ContactsRepository.UpdateAsync(_mapper.Map<ContactEntity>(contact), cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Success;
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(ContactService))
                .WithMethod(nameof(UpdateAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(nameof(Guid), nameof(userId), userId.ToString())
                .WithParameter(typeof(ContactDomain).FullName ?? String.Empty, nameof(contact), contact.SerializeObject())
                .ToString());
            
            throw;
        }
    }

    private static bool ValidateUserAccessToContact(Guid contactId, UserEntity user)
    {
        if (user.Contacts is null)
        {
            throw new InvalidOperationException("User contacts is null. This should not happen.");
        }
        
        return user.Contacts.Any(x => x.Id == contactId);
    }
}
