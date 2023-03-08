using LanguageExt.Common;
using PropertySearchApp.Domain;

namespace PropertySearchApp.Services.Abstract;

public interface IIdentityService
{
    Task<Result<bool>> RegisterAsync(UserDomain user);
    Task<Result<bool>> LoginAsync(string email, string password);
    Task SignOutAsync();
}