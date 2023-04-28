using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PropertySearchApp.Entities;
using PropertySearchApp.Repositories.Abstract;

namespace PropertySearchApp.Repositories;

public class UserRepository : IUserRepository, IUserReceiverRepository
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly ILogger<UserRepository> _logger;
    public UserRepository(UserManager<UserEntity> userManager, ILogger<UserRepository> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<IdentityResult> AddToRoleAsync(UserEntity user, string roleName)
    {
        ValidateUserIfInvalidThrowException(user);
        ValidateStringIfInvalidThrowException(nameof(roleName), roleName);
        return await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task<bool> CheckPasswordAsync(UserEntity user, string password)
    {
        ValidateUserIfInvalidThrowException(user);
        ValidateStringIfInvalidThrowException(nameof(password), password);
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IdentityResult> CreateAsync(UserEntity user, string password)
    {
        ValidateUserIfInvalidThrowException(user);
        ValidateStringIfInvalidThrowException(nameof(password), password);
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<UserEntity?> FindByEmailAsync(string email)
    {
        ValidateStringIfInvalidThrowException(nameof(email), email);
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id)
    {
        return await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<UserEntity?> GetByIdWithAccommodationsAsync(Guid userId)
    {
        return await _userManager.Users.Include(x => x.Accommodations).FirstOrDefaultAsync(x => x.Id == userId);
    }
    public async Task<UserEntity?> GetByIdWithContactsAsync(Guid userId)
    {
        return await _userManager.Users.Include(x => x.Contacts).FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<IdentityResult> UpdateFieldsAsync(UserEntity user, string newUsername, string newInformation)
    {
        ValidateUserIfInvalidThrowException(user);
        ValidateStringIfInvalidThrowException(nameof(newUsername), newUsername);
        ValidateStringIfInvalidThrowException(nameof(newInformation), newInformation);

        user.UserName = newUsername;
        user.Information = newInformation;

        return await _userManager.UpdateAsync(user);
    }

    private void ValidateUserIfInvalidThrowException(UserEntity user)
    {
        if (user == null)
        {
            ThrowArgumentException<ArgumentNullException>($"{nameof(user)} can not be null");
        }

        ValidateStringIfInvalidThrowException(nameof(user.UserName), user.UserName);
        ValidateStringIfInvalidThrowException(nameof(user.Email), user.Email);
    }

    private void ValidateStringIfInvalidThrowException(string nameOfProperty, string stringToValidate)
    {
        if (string.IsNullOrEmpty(stringToValidate))
        {
            ThrowArgumentException<ArgumentException>($"{nameOfProperty} can not be null or empty");
        }
    }

    private void ThrowArgumentException<TException>(string exceptionMessage)
        where TException : ArgumentException, new()
    {
        var exception = (TException)Activator.CreateInstance(typeof(TException), exceptionMessage);
        _logger.LogError(exceptionMessage);
        throw exception == null ? throw new NullReferenceException("Can not cast exception to argument exception")
            : exception;
    }

    public async Task<IdentityResult> ChangePasswordAsync(UserEntity user, string currentPassword, string newPassword)
    {
        return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }
}
