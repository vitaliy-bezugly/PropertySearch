using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using PropertySearchApp.Domain;

namespace PropertySearchApp.Services.Abstract;

public interface IIdentityService
{
    Task<Result<bool>> RegisterAsync(UserDomain user);
    Task<Result<bool>> LoginAsync(string username, string password);
    Task SignOutAsync();
    Task<UserDomain> GetUserByIdAsync(Guid id);
    Task<Result<bool>> UpdateUser(UserDomain user);
}