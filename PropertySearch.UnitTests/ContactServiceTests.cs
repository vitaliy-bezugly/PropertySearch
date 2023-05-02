using AutoMapper;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using PropertySearchApp.Common;
using PropertySearchApp.Common.Constants;
using PropertySearchApp.Domain;
using PropertySearchApp.Entities;
using PropertySearchApp.Repositories.Abstract;
using PropertySearchApp.Services;
using PropertySearchApp.Services.Abstract;

namespace PropertySearch.UnitTests;

public class ContactServiceTests
{
    private readonly IContactService _sut;

    private readonly IUserReceiverRepository _userReceiverRepository = Substitute.For<IUserReceiverRepository>();
    private readonly IContactsRepository _contactsRepository = Substitute.For<IContactsRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    public ContactServiceTests()
    {
        _sut = new ContactService(_userReceiverRepository, _contactsRepository, _mapper);
    }

    [Fact]
    public async Task GetUserContacts_ShouldReturnOneContacts_WhenContactsExists()
    {
        // Arrange
        Guid userId = Guid.Parse("06012f2e-caeb-4504-9685-10fde03552de"), contactId = Guid.Parse("c98d1be7-3a01-4b33-aeab-9a39b147f865");
        string type = "Email address", email = "user@email.com";
        var contacts = new List<ContactEntity> 
        {
            new ContactEntity
            {
                Id = contactId,
                ContactType = type,
                Content = email,
                UserId = userId,
                CreationTime = DateTime.Now
            }
        };

        _contactsRepository.GetUserContactsAsync(userId).Returns(contacts);
        _mapper.Map<ContactDomain>(contacts.First()).Returns(new ContactDomain
        {
            Id = contactId,
            ContactType = type,
            Content = email
        });
        // Act
        var actual = await _sut.GetUserContactsAsync(userId);

        // Assert
        actual.Should().HaveCount(1);
    }
    [Fact]
    public async Task GetUserContacts_ShouldReturnEmptyList_WhenContactsDoesNotExist()
    {
        // Arrange
        Guid userId = Guid.Parse("06012f2e-caeb-4504-9685-10fde03552de");

        _contactsRepository.GetUserContactsAsync(userId).ReturnsNull();
        // Act
        var actual = await _sut.GetUserContactsAsync(userId);

        // Assert
        actual.Should().BeEmpty();
    }
    [Fact]
    public async Task AddContactToUser_ShouldAddContact_WhenDataIsValidAndUserExists()
    {
        // Arrange
        Guid userId = Guid.Parse("06012f2e-caeb-4504-9685-10fde03552de"), contactId = Guid.Parse("c98d1be7-3a01-4b33-aeab-9a39b147f865");
        string type = "Email address", email = "user@email.com";
        var contact = new ContactEntity
        {
            Id = contactId,
            ContactType = type,
            Content = email,
            UserId = userId,
            CreationTime = DateTime.Now
        };
        var contactDomain = new ContactDomain
        {
            Id = contactId,
            ContactType = type,
            Content = email
        };

        _userReceiverRepository.GetByIdWithContactsAsync(userId).Returns(new UserEntity
        {
            Id = userId,
            Contacts = new List<ContactEntity>()
        });
        _contactsRepository.AddContactToUserAsync(userId, contact).Returns(new OperationResult());
        _mapper.Map<ContactEntity>(contactDomain).Returns(contact);

        // Act
        var actual = await _sut.AddContactToUserAsync(userId, contactDomain);

        // Assert
        actual.Succeeded.Should().Be(true);
        actual.ErrorMessage.Should().BeNullOrEmpty();
    }
    [Fact]
    public async Task AddContactToUser_ShouldNotAddContact_WhenUserDoesNotExist()
    {
        // Arrange
        Guid userId = Guid.Parse("06012f2e-caeb-4504-9685-10fde03552de"), contactId = Guid.Parse("c98d1be7-3a01-4b33-aeab-9a39b147f865");
        string type = "Email address", email = "user@email.com";
        var contact = new ContactEntity
        {
            Id = contactId,
            ContactType = type,
            Content = email,
            UserId = userId,
            CreationTime = DateTime.Now
        };
        var contactDomain = new ContactDomain
        {
            Id = contactId,
            ContactType = type,
            Content = email
        };

        _userReceiverRepository.GetByIdWithContactsAsync(userId).ReturnsNull();

        // Act
        var actual = await _sut.AddContactToUserAsync(userId, contactDomain);

        // Assert
        actual.Succeeded.Should().Be(false);
        actual.ErrorMessage.Should().Be(ErrorMessages.User.NotFound);
    }
    [Fact]
    public async Task AddContactToUser_ShouldNotAddContact_WhenUserAlreadyHasSame()
    {
        // Arrange
        Guid userId = Guid.Parse("06012f2e-caeb-4504-9685-10fde03552de"), contactId = Guid.Parse("c98d1be7-3a01-4b33-aeab-9a39b147f865");
        string type = "Email address", email = "user@email.com";
        var contact = new ContactEntity
        {
            Id = contactId,
            ContactType = type,
            Content = email,
            UserId = userId,
            CreationTime = DateTime.Now
        };
        var contactDomain = new ContactDomain
        {
            Id = contactId,
            ContactType = type,
            Content = email
        };

        _userReceiverRepository.GetByIdWithContactsAsync(userId).Returns(new UserEntity
        {
            Id = userId,
            Contacts = new List<ContactEntity> { contact }
        });

        // Act
        var actual = await _sut.AddContactToUserAsync(userId, contactDomain);

        // Assert
        actual.Succeeded.Should().Be(false);
        actual.ErrorMessage.Should().Be(ErrorMessages.Contacts.AlreadyExist);
    }
    [Fact]
    public async Task DeleteContactFromUser_ShouldDeleteContact_WhenUserExist()
    {
        // Arrange
        Guid userId = Guid.Parse("06012f2e-caeb-4504-9685-10fde03552de"), contactId = Guid.Parse("c98d1be7-3a01-4b33-aeab-9a39b147f865");
        string type = "Email address", email = "user@email.com";
        var contact = new ContactEntity
        {
            Id = contactId,
            ContactType = type,
            Content = email,
            UserId = userId,
            CreationTime = DateTime.Now
        };
        var contactDomain = new ContactDomain
        {
            Id = contactId,
            ContactType = type,
            Content = email
        };

        _userReceiverRepository.GetByIdWithContactsAsync(userId).Returns(new UserEntity
        {
            Id = userId,
            Contacts = new List<ContactEntity>()
        });
        _contactsRepository.DeleteContactAsync(contactId)
            .Returns(new OperationResult());

        // Act
        var actual = await _sut.DeleteContactFromUserAsync(userId, contactId);

        // Assert
        actual.Succeeded.Should().Be(true);
        actual.ErrorMessage.Should().BeNullOrEmpty();
    }
    [Fact]
    public async Task DeleteContactFromUser_ShouldNotDeleteContact_WhenUserDoesNotExist()
    {
        // Arrange
        Guid userId = Guid.Parse("06012f2e-caeb-4504-9685-10fde03552de"), contactId = Guid.Parse("c98d1be7-3a01-4b33-aeab-9a39b147f865");
        string type = "Email address", email = "user@email.com";
        var contact = new ContactEntity
        {
            Id = contactId,
            ContactType = type,
            Content = email,
            UserId = userId,
            CreationTime = DateTime.Now
        };
        var contactDomain = new ContactDomain
        {
            Id = contactId,
            ContactType = type,
            Content = email
        };

        _userReceiverRepository.GetByIdWithContactsAsync(userId)
            .ReturnsNull();

        // Act
        var actual = await _sut.DeleteContactFromUserAsync(userId, contactId);

        // Assert
        actual.Succeeded.Should().Be(false);
        actual.ErrorMessage.Should().Be(ErrorMessages.User.NotFound);
    }
}
