using Microsoft.AspNetCore.Identity;
using PropertySearchApp.Entities;

namespace PropertySearchApp.Repositories.Abstract;

public interface IUserRepository
{
    Task<UserEntity?> FindByEmailAsync(string email);
    Task<IdentityResult> CreateAsync(UserEntity user, string password);
    /// <summary>
    /// Method that updates the username, info, and contacts fields
    /// </summary>
    /// <param name="user">Have to be with contacts and as tracking</param>
    /// <param name="newUsername">Future username of given user</param>
    /// <param name="newInformation">Future information about given user</param>
    /// <param name="newContacts">Future contacts of given user. Con not be null</param>
    /// <returns></returns>
    Task<IdentityResult> UpdateFieldsAsync(UserEntity user, string newUsername, string newInformation);
    Task<IdentityResult> AddToRoleAsync(UserEntity user, string roleName);
    Task<bool> CheckPasswordAsync(UserEntity user, string password);
    Task<IdentityResult> ChangePasswordAsync(UserEntity user, string currentPassword, string newPassword);
}