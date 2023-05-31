using PropertySearchApp.Common;
using PropertySearchApp.Domain;

namespace PropertySearchApp.Services.Abstract;

public interface IIdentityService
{
    Task SignOutAsync();
    Task<UserDomain?> GetUserByIdAsync(Guid id);
    Task<OperationResult> RegisterAsync(UserDomain user);
    Task<OperationResult> LoginAsync(string username, string password);
    Task<OperationResult> UpdateUserFieldsAsync(Guid userId, string newUsername, string newInformation, string password);
    Task<OperationResult> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
    Task<OperationResult> ConfirmEmailAsync(Guid userId, string token);
    Task<bool> IsEmailConfirmedAsync(Guid userId);
    Task SendConfirmationEmailAsync(Guid userId);
}