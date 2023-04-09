using Microsoft.AspNetCore.Identity;
using PropertySearchApp.Entities;
using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Services;

public class SignInService : ISignInService
{
    private readonly SignInManager<UserEntity> _signInManager;
    public SignInService(SignInManager<UserEntity> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<SignInResult> PasswordSignInAsync(string username, string password, bool isPersistent, bool lockOutOnFailure)
    {
        return await _signInManager.PasswordSignInAsync(username, password, isPersistent, lockOutOnFailure);
    }

    public async Task SignInAsync(UserEntity user, bool isPersistent)
    {
        await _signInManager.SignInAsync(user, isPersistent);    
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}
