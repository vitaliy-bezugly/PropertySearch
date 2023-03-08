using AutoMapper;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using PropertySearchApp.Common.Exceptions;
using PropertySearchApp.Domain;
using PropertySearchApp.Entities;
using PropertySearchApp.Persistence.Exceptions;
using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly ILogger<IdentityService> _logger;
    private readonly IMapper _mapper;
    public IdentityService(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, RoleManager<IdentityRole<Guid>> roleManager, ILogger<IdentityService> logger, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<bool>> RegisterAsync(UserDomain user)
    {
        var userEntity = _mapper.Map<UserEntity>(user);
        
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
            await SetRoleToUserAsync("user", userEntity);
            if (user.IsLandlord == true)
            {
                await SetRoleToUserAsync("landlord", userEntity);
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
    private async Task SetRoleToUserAsync(string roleName, UserEntity user)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        
        if (role == null)
        {
            var exception = new NotFoundRoleException($"Can not find role: {roleName}");
            _logger.LogCritical(exception, "Can not find role");
            throw exception;
        }

        IdentityResult roleAddingResult = await _userManager.AddToRoleAsync(user, role.Name);
        if(roleAddingResult.Succeeded == false)
        {
            var exception = new RegistrationOperationException(roleAddingResult.Errors.Select(x => x.Description).ToArray());
            _logger.LogCritical(exception, "Can not add role to user");
            throw exception;
        }
    }
}