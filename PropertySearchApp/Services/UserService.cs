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
    public UserService(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public async Task<Result<bool>> RegisterAsync(UserDomain user)
    {
        var userToRegister = new UserEntity
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
        var result = await _userManager.CreateAsync(userToRegister, user.Password);
        if (result.Succeeded)
        {
            // Set roles 
            /*
            string roleName = "landlord";
            var response = await SetRoleToUserAsync(roleName, userToRegister);
            response.Match<bool>(success =>
            {
                return true;
            }, exception =>
            {
                if (exception is NotFoundRoleException registrationException)
                {
                    var creationRoleResult = await CreateRoleAsync(roleName);
                    return true;
                }

                return false;
            });
             */

            // Set cookies
            await _signInManager.SignInAsync(userToRegister, false);
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
            return new Result<bool>(new NotFoundRoleException("Can not find role with this name"));
        }

        IdentityResult roleresult = await _userManager.AddToRoleAsync(user, role.Name);
        return new Result<bool>(true);
    }

    private async Task<IdentityResult> CreateRoleAsync(string roleName)
    {
        return await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
    }
}