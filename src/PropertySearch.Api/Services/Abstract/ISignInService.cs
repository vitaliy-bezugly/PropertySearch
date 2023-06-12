using Microsoft.AspNetCore.Identity;
using PropertySearch.Api.Entities;

namespace PropertySearch.Api.Services.Abstract;

public interface ISignInService
{
    Task SignInAsync(UserEntity user, bool isPersistent);
    Task SignOutAsync();
    Task<SignInResult> PasswordSignInAsync(string username, string password, bool isPersistent, bool lockOutOnFailure);
}