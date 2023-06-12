using Microsoft.AspNetCore.Identity;
using PropertySearch.Api.Entities;

namespace PropertySearch.Api.Repositories.Abstract;

public interface IUserRepository
{
    Task<UserEntity?> FindByEmailAsync(string email);
    Task<IdentityResult> CreateAsync(UserEntity user, string password);
    Task<IdentityResult> UpdateFieldsAsync(UserEntity user, string newUsername, string newInformation);
    Task<IdentityResult> AddToRoleAsync(UserEntity user, string roleName);
    Task<bool> CheckPasswordAsync(UserEntity user, string password);
    Task<IdentityResult> ChangePasswordAsync(UserEntity user, string currentPassword, string newPassword);
    Task<IdentityResult> ConfirmEmailAsync(UserEntity user, string token);
}