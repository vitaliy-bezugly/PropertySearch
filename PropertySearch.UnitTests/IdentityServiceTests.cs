using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using PropertySearchApp.Common.Constants;
using PropertySearchApp.Domain;
using PropertySearchApp.Entities;
using PropertySearchApp.Repositories.Abstract;
using PropertySearchApp.Services;
using PropertySearchApp.Services.Abstract;

namespace PropertySearch.UnitTests;

public class IdentityServiceTests
{
    private readonly IdentityService _sut;

    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IRoleRepository _roleRepository = Substitute.For<IRoleRepository>();
    private readonly ISignInService _signInService = Substitute.For<ISignInService>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IUserReceiverRepository _userReceiver = Substitute.For<IUserReceiverRepository>();
    private readonly ILogger<IdentityService> _logger = Substitute.For<ILogger<IdentityService>>();

    public IdentityServiceTests()
    {
        _sut = new IdentityService(_userRepository, _signInService, _roleRepository, _logger, _mapper, _userReceiver);
    }

    [Fact]
    public async Task Register_ShouldRegisterUser_WhenAllParametersAreValid()
    {
        // Arrange
        string userRoleName = "user";
        var user = new UserDomain { Id = Guid.NewGuid(), Username = "System Under Test", Email = "unit@test.com", IsLandlord = false, Password = "qwerty123", Information = string.Empty };
        var entity = new UserEntity { Id = user.Id, UserName = user.Username, Email = user.Email, IsLandlord = user.IsLandlord, PasswordHash = string.Empty, Information = user.Information, Contacts = new List<ContactEntity>() };
        _mapper.Map<UserEntity>(user).Returns(entity);
        _userRepository.FindByEmailAsync(entity.Email).ReturnsNull();
        _userRepository.CreateAsync(entity, user.Password).Returns(IdentityResult.Success);
        _roleRepository.FindByNameAsync(userRoleName).Returns(new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = userRoleName, NormalizedName = userRoleName.ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() });
        _userRepository.AddToRoleAsync(entity, userRoleName).Returns(IdentityResult.Success);

        // Act
        var actual = await _sut.RegisterAsync(user);

        // Assert
        actual.Succeeded.Should().Be(true);
        await _userRepository.Received(1).CreateAsync(Arg.Any<UserEntity>(), Arg.Any<string>());
    }
    [Fact]
    public async Task Register_ShouldNotRegisterUser_WhenUserWithSameEmailAlreadyExists()
    {
        // Arrange
        string userRoleName = "user";
        var user = new UserDomain { Id = Guid.NewGuid(), Username = "System Under Test", Email = "unit@test.com", IsLandlord = false, Password = "qwerty123", Information = string.Empty };
        var entity = new UserEntity { Id = user.Id, UserName = user.Username, Email = user.Email, IsLandlord = user.IsLandlord, PasswordHash = string.Empty, Information = user.Information, Contacts = new List<ContactEntity>() };
        _mapper.Map<UserEntity>(user).Returns(entity);
        _userRepository.FindByEmailAsync(entity.Email).Returns(new UserEntity { Id = Guid.NewGuid() });

        // Act
        var actual = await _sut.RegisterAsync(user);

        // Assert
        actual.Succeeded.Should().Be(false);
        actual.ErrorMessage.Should().BeEquivalentTo(ErrorMessages.User.SameEmail);
    }
    [Fact]
    public async Task Login_ShouldLogin_WhenAllParametersAreValid()
    {
        // Arrange
        string username = "xunit", password = "correctpassword";
        _signInService.PasswordSignInAsync(username, password, false, false).Returns(SignInResult.Success);
        // Act
        var actual = await _sut.LoginAsync(username, password);

        // Assert
        actual.Succeeded.Should().Be(true);
        await _signInService.Received(1).PasswordSignInAsync(username, password, false, false);
    }
    [Fact]
    public async Task Login_ShouldNotLogin_WhenCredentialsIsWrong()
    {
        // Arrange
        string username = "xunit", password = "incorrectpassword";
        _signInService.PasswordSignInAsync(username, password, false, false).Returns(SignInResult.Failed);
        // Act
        var actual = await _sut.LoginAsync(username, password);

        // Assert
        actual.Succeeded.Should().Be(false);
        actual.ErrorMessage.Should().BeEquivalentTo(ErrorMessages.User.WrongCredentials);
        await _signInService.Received(1).PasswordSignInAsync(username, password, false, false);
    }
    [Fact]
    public async Task GetUserById_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var user = new UserDomain { Id = Guid.NewGuid(), Username = "System Under Test", Email = "unit@test.com", IsLandlord = false, Password = "qwerty123", Information = string.Empty };
        var entity = new UserEntity { Id = user.Id, UserName = user.Username, Email = user.Email, IsLandlord = user.IsLandlord, PasswordHash = string.Empty, Information = user.Information, Contacts = new List<ContactEntity>() };
        _userReceiver.GetByIdAsync(user.Id).Returns(entity);
        _mapper.Map<UserDomain>(entity).Returns(user);

        // Act
        var actual = await _sut.GetUserByIdAsync(user.Id);

        // Assert
        actual.Should().NotBeNull();
    }
    [Fact]
    public async Task GetUserById_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        var user = new UserDomain { Id = Guid.NewGuid(), Username = "System Under Test", Email = "unit@test.com", IsLandlord = false, Password = "qwerty123", Information = string.Empty };
        _userReceiver.GetByIdAsync(user.Id).ReturnsNull();

        // Act
        var actual = await _sut.GetUserByIdAsync(user.Id);

        // Assert
        actual.Should().BeNull();
    }
    [Fact]
    public async Task UpdateUserFields_ShouldUpdateFields_WhenAllParametersAreValid()
    {
        // Arrange
        string newUsername = "Changed username", newInformation = "Some information";
        var contacts = new List<ContactDomain> { new ContactDomain { Id = Guid.NewGuid(), ContactType = "Email address", Content = "unit@test.com" } };
        var contactsEntity = contacts.Select(x => new ContactEntity { Id = x.Id, Content = x.Content, ContactType = x.ContactType, CreationTime = DateTime.Now }).ToList();

        var user = new UserDomain { Id = Guid.NewGuid(), Username = "System Under Test", Email = "unit@test.com", IsLandlord = false, Password = "qwerty123", Information = string.Empty };
        var userInDatabase = new UserEntity { Id = user.Id, UserName = user.Username, Email = user.Email, IsLandlord = user.IsLandlord, PasswordHash = string.Empty, Information = user.Information, Contacts = new List<ContactEntity>() };
        _userReceiver.GetByIdWithContactsAsync(user.Id).Returns(userInDatabase);
        _userRepository.CheckPasswordAsync(userInDatabase, user.Password).Returns(true);
        _userRepository.UpdateFieldsAsync(userInDatabase, newUsername, newInformation).Returns(IdentityResult.Success);
        _mapper.Map<ContactEntity>(contacts[0]).Returns(contactsEntity[0]);

        // Act
        var actual = await _sut.UpdateUserFieldsAsync(user.Id, newUsername, newInformation, user.Password);

        // Assert
        actual.Succeeded.Should().Be(true);
        await _userRepository.Received(1).UpdateFieldsAsync(Arg.Any<UserEntity>(), Arg.Any<string>(), Arg.Any<string>());
    }
    [Fact]
    public async Task UpdateUserFields_ShouldNotUpdateFields_WhenUserDoesNotExist()
    {
        // Arrange
        string newUsername = "Changed username", newInformation = "Some information";
        var user = new UserDomain { Id = Guid.NewGuid(), Username = "System Under Test", Email = "unit@test.com", IsLandlord = false, Password = "qwerty123", Information = string.Empty };
        _userReceiver.GetByIdWithContactsAsync(user.Id).ReturnsNull();

        // Act
        var actual = await _sut.UpdateUserFieldsAsync(user.Id, newUsername, newInformation, user.Password);

        // Assert
        actual.Succeeded.Should().Be(false);
        actual.ErrorMessage.Should().BeEquivalentTo(ErrorMessages.User.NotFound);
        await _userReceiver.Received(1).GetByIdWithContactsAsync(Arg.Any<Guid>());
    }
    [Fact]
    public async Task UpdateUserFields_ShouldNotUpdateFields_WhenPasswordIsWrong()
    {
        // Arrange
        string newUsername = "Changed username", newInformation = "Some information";
        string wrongPassword = "wrongpassword";
        var user = new UserDomain { Id = Guid.NewGuid(), Username = "System Under Test", Email = "unit@test.com", IsLandlord = false, Password = wrongPassword, Information = string.Empty };
        var entity = new UserEntity { Id = user.Id, UserName = user.Username, Email = user.Email, IsLandlord = user.IsLandlord, PasswordHash = string.Empty, Information = user.Information, Contacts = new List<ContactEntity>() };
        _userReceiver.GetByIdWithContactsAsync(user.Id).Returns(entity);
        _userRepository.CheckPasswordAsync(entity, wrongPassword).Returns(false);

        // Act
        var actual = await _sut.UpdateUserFieldsAsync(user.Id, newUsername, newInformation, user.Password);

        // Assert
        actual.Succeeded.Should().Be(false);
        actual.ErrorMessage.Should().BeEquivalentTo(ErrorMessages.User.WrongPassword);
        await _userReceiver.Received(1).GetByIdWithContactsAsync(Arg.Any<Guid>());
        await _userRepository.Received(1).CheckPasswordAsync(entity, wrongPassword);
    }
}