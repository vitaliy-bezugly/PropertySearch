using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using PropertySearchApp.Common.Exceptions;
using PropertySearchApp.Domain;
using PropertySearchApp.Entities;
using PropertySearchApp.Persistence.Exceptions;
using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Services;

public class UserService : IUserService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<UserService> _logger;
    public UserService(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, RoleManager<IdentityRole> roleManager, ILogger<UserService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<Result<bool>> RegisterAsync(UserDomain user)
    {
        var userEntity = new UserEntity
        {
            UserName = user.Username,
            Email = user.Email
        };
        
        var exists = await _userManager.FindByEmailAsync(user.Email);
        if (exists != null)
        {
            var badResult = new Result<bool>(new RegistrationOperationException(new []{ "User with same email already exists" }));
            return badResult;
        }
        
        // Add user
        var result = await _userManager.CreateAsync(userEntity, user.Password);
        if (result.Succeeded)
        {
            // Set roles 
            if(user.IsLandlord == true)
            {
                Result<bool> roleAddingResult = await SetRoleToUserAsync("landlord", userEntity);

                if (roleAddingResult.IsSuccess == false)
                    return roleAddingResult;
            }

            // Set cookies
            await _signInManager.SignInAsync(userEntity, false);
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

    private async Task<Result<bool>> SetRoleToUserAsync(string roleName, UserEntity user)
    {
        var role = await _roleManager.FindByNameAsync(roleName);

        if (role == null)
        {
            var exception = new NotFoundRoleException($"Can not find role: {roleName}");
            _logger.LogCritical(exception, "Can not find role");

            return new Result<bool>(new NotFoundRoleException($"Can not find role: {roleName}"));
        }

        IdentityResult roleAddingResult = await _userManager.AddToRoleAsync(user, role.Name);
        if(roleAddingResult.Succeeded)
        {
            return new Result<bool>(true);
        }
        else
        {
            var exception = new RegistrationOperationException(roleAddingResult.Errors.Select(x => x.Description).ToArray());
            _logger.LogCritical(exception, "Can not add role to user");
            return new Result<bool>(exception);
        }
    }
}