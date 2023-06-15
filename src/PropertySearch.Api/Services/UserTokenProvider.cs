using Microsoft.AspNetCore.Identity;
using PropertySearch.Api.Entities;
using PropertySearch.Api.Services.Abstract;

namespace PropertySearch.Api.Services;

public class UserTokenProvider : ITokenProvider
{
    private readonly UserManager<UserEntity> _userManager;

    public UserTokenProvider(UserManager<UserEntity> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(UserEntity user)
    {
        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        return token;
    }
}