using Microsoft.AspNetCore.Identity;
using PropertySearchApp.Common.Extensions;
using PropertySearchApp.Common.Logging;
using PropertySearchApp.Entities;
using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Services;

public class SignInService : ISignInService
{
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly ILogger<SignInService> _logger;
    public SignInService(SignInManager<UserEntity> signInManager, ILogger<SignInService> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<SignInResult> PasswordSignInAsync(string username, string password, bool isPersistent, bool lockOutOnFailure)
    {
        try
        {
            return await _signInManager.PasswordSignInAsync(username, password, isPersistent, lockOutOnFailure);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(SignInService))
                .WithMethod(nameof(PasswordSignInAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(nameof(String), nameof(username), username)
                .WithParameter(nameof(String), nameof(password), password)
                .WithParameter(nameof(Boolean), nameof(isPersistent), isPersistent.ToString())
                .WithParameter(nameof(Boolean), nameof(lockOutOnFailure), lockOutOnFailure.ToString())
                .ToString());
            
            throw;
        }
    }

    public async Task SignInAsync(UserEntity user, bool isPersistent)
    {
        try
        {
            await _signInManager.SignInAsync(user, isPersistent);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(SignInService))
                .WithMethod(nameof(SignInAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(nameof(Boolean), nameof(isPersistent), isPersistent.ToString())
                .WithParameter(typeof(UserEntity).FullName ?? String.Empty, nameof(user), user.SerializeObject())
                .ToString());
            
            throw;
        }
    }

    public async Task SignOutAsync()
    {
        try
        {
            await _signInManager.SignOutAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(SignInService))
                .WithMethod(nameof(SignOutAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithNoParameters()
                .ToString());
            
            throw;
        }
    }
}
