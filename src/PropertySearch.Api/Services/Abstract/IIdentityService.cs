using PropertySearch.Api.Common;
using PropertySearch.Api.Domain;

namespace PropertySearch.Api.Services.Abstract;

public interface IIdentityService
{
    Task SignOutAsync();
    Task<UserDomain?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<OperationResult> RegisterAsync(UserDomain user, CancellationToken cancellationToken);
    Task<OperationResult> LoginAsync(string username, string password);
    Task<OperationResult> UpdateUserFieldsAsync(Guid userId, string newUsername, string newInformation, string password, CancellationToken cancellationToken);
    Task<OperationResult> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, CancellationToken cancellationToken);
    Task<OperationResult> ConfirmEmailAsync(Guid userId, string token, CancellationToken cancellationToken);
    Task<bool> IsEmailConfirmedAsync(Guid userId, CancellationToken cancellationToken);
    Task SendConfirmationEmailAsync(Guid userId, CancellationToken cancellationToken);
}