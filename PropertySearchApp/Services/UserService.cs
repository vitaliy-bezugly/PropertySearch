using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using PropertySearchApp.Common.Exceptions;
using PropertySearchApp.Entities;
using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Services;

public class UserService : IUserService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager;
    public UserService(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<Result<bool>> RegisterAsync(string username, string email, string password)
    {
        var user = new UserEntity
        {
            UserName = username,
            Email = email
        };
        
        var exists = await _userManager.FindByEmailAsync(user.Email);
        if (exists != null)
        {
            var badResult = new Result<bool>(new RegistrationOperationException(new []{ "User with same email already exists" }));
            return badResult;
        }
        
        // Add user
        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            // Set cookies
            await _signInManager.SignInAsync(user, false);
            return new Result<bool>(true);
        }
        else
        {
            return new Result<bool>(new RegistrationOperationException(result.Errors.Select(x => x.Description).ToArray()));
        }
    }
    public async Task<Result<bool>> LoginAsync(string email, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

        if (result.Succeeded == true)
        {
            return new Result<bool>(true);
        }
        else
        {
            return new Result<bool>(new LoginOperationException(new[] { "User with this email address and password does not exist" }));
        }
    }
    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}