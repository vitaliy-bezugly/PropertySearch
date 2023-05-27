﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PropertySearchApp.Common.Extensions;
using PropertySearchApp.Common.Logging;
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
        try
        {
            ValidateUserIfInvalidThrowException(user);
            ValidateStringIfInvalidThrowException(nameof(roleName), roleName);
            return await _userManager.AddToRoleAsync(user, roleName);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(UserRepository))
                .WithMethod(nameof(AddToRoleAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(typeof(UserEntity).FullName, nameof(user), user.SerializeObject())
                .WithParameter(typeof(string).Name, nameof(roleName), roleName)
                .ToString());
            
            throw;
        }
    }

    public async Task<bool> CheckPasswordAsync(UserEntity user, string password)
    {
        try
        {
            ValidateUserIfInvalidThrowException(user);
            ValidateStringIfInvalidThrowException(nameof(password), password);
            return await _userManager.CheckPasswordAsync(user, password);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(UserRepository))
                .WithMethod(nameof(CheckPasswordAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(typeof(UserEntity).FullName, nameof(user), user.SerializeObject())
                .WithParameter(typeof(string).Name, nameof(password), password)
                .ToString());
            
            throw;
        }
    }

    public async Task<IdentityResult> CreateAsync(UserEntity user, string password)
    {
        try
        {
            ValidateUserIfInvalidThrowException(user);
            ValidateStringIfInvalidThrowException(nameof(password), password);
            return await _userManager.CreateAsync(user, password);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(UserRepository))
                .WithMethod(nameof(CreateAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(typeof(UserEntity).FullName, nameof(user), user.SerializeObject())
                .WithParameter(typeof(string).Name, nameof(password), password)
                .ToString());

            throw;
        }
    }

    public async Task<UserEntity?> FindByEmailAsync(string email)
    {
        try
        {
            ValidateStringIfInvalidThrowException(nameof(email), email);
            return await _userManager.FindByEmailAsync(email);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(UserRepository))
                .WithMethod(nameof(FindByEmailAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(typeof(string).Name, nameof(email), email)
                .ToString());

            throw;
        }
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(UserRepository))
                .WithMethod(nameof(GetByIdAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(typeof(Guid).Name, nameof(id), id.ToString())
                .ToString());

            throw;
        }
    }
    public async Task<UserEntity?> GetByIdWithAccommodationsAsync(Guid userId)
    {
        try
        {
            return await _userManager.Users.Include(x => x.Accommodations).FirstOrDefaultAsync(x => x.Id == userId);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(UserRepository))
                .WithMethod(nameof(GetByIdWithAccommodationsAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(typeof(Guid).Name, nameof(userId), userId.ToString())
                .ToString());
            
            throw;
        }
    }
    public async Task<UserEntity?> GetByIdWithContactsAsync(Guid userId)
    {
        try
        {
            return await _userManager.Users.Include(x => x.Contacts).FirstOrDefaultAsync(x => x.Id == userId);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(UserRepository))
                .WithMethod(nameof(GetByIdWithContactsAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(typeof(Guid).Name, nameof(userId), userId.ToString())
                .ToString());
            
            throw;
        }
    }

    public async Task<IdentityResult> UpdateFieldsAsync(UserEntity user, string newUsername, string newInformation)
    {
        try
        {
            ValidateUserIfInvalidThrowException(user);
            ValidateStringIfInvalidThrowException(nameof(newUsername), newUsername);
            ValidateStringIfInvalidThrowException(nameof(newInformation), newInformation);

            user.UserName = newUsername;
            user.Information = newInformation;

            return await _userManager.UpdateAsync(user);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(UserRepository))
                .WithMethod(nameof(UpdateFieldsAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(typeof(UserEntity).FullName, nameof(user), user.SerializeObject())
                .WithParameter(typeof(string).Name, nameof(newUsername), newUsername)
                .WithParameter(typeof(string).Name, nameof(newInformation), newInformation)
                .ToString());
            
            throw;
        }
    }
    
    public async Task<IdentityResult> ChangePasswordAsync(UserEntity user, string currentPassword, string newPassword)
    {
        try
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(UserRepository))
                .WithMethod(nameof(AddToRoleAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(typeof(UserEntity).FullName, nameof(user), user.SerializeObject())
                .WithParameter(typeof(string).Name, nameof(currentPassword), currentPassword)
                .WithParameter(typeof(string).Name, nameof(newPassword), newPassword)
                .ToString());
            
            throw;
        }
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
}
