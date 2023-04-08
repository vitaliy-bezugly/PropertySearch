using AutoMapper;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Pipes;
using Microsoft.AspNetCore.Identity;
using PropertySearchApp.Common.Exceptions;
using PropertySearchApp.Domain;
using PropertySearchApp.Entities;
using PropertySearchApp.Persistence.Exceptions;
using PropertySearchApp.Repositories.Abstract;
using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly ILogger<IdentityService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserReceiverRepository _userReceiverRepository;
    public IdentityService(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, RoleManager<IdentityRole<Guid>> roleManager, ILogger<IdentityService> logger, IMapper mapper, IUserReceiverRepository userReceiverRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _logger = logger;
        _mapper = mapper;
        _userReceiverRepository = userReceiverRepository;
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
    public async Task<Result<bool>> LoginAsync(string username, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

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
    public async Task<UserDomain?> GetUserByIdAsync(Guid userId)
    {
        var entity = await _userReceiverRepository.GetByIdAsync(userId);
        return entity == null ? null : _mapper.Map<UserDomain>(entity);
    }
    public async Task<Result<bool>> UpdateUserFields(UserDomain user)
    {
        var entity = await _userReceiverRepository.GetByIdWithContactsAsync(user.Id);
        if (entity == null)
        {
            var badResult = new Result<bool>(new UserNotFoundException(new[] { "User with given id does not exist" }));
            return badResult;
        }
        
        /* Validate password */
        var givenPasswordSameToActual = await _userManager.CheckPasswordAsync(entity, user.Password);
        if(givenPasswordSameToActual == false)
        {
            var badResult = new Result<bool>(new UserNotFoundException(new[] { "Given password and actual are not the same" }));
            return badResult;
        }
        
        /* Update user fields */
        entity.UserName = user.Username;
        entity.Information = user.Information;
        entity.Contacts = user.Contacts.Select(x => _mapper.Map<ContactEntity>(x)).ToList();

        var result = await _userManager.UpdateAsync(entity);
        return result.Succeeded ? new Result<bool>(true) : 
            new Result<bool>(new UserUpdateOperationException(result.Errors
                .Select(x => x.Description).ToArray()));
    }
}