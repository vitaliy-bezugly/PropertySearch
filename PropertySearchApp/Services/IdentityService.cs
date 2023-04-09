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
    private readonly IMapper _mapper;
    private readonly ISignInService _signInService;
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<IdentityService> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IUserReceiverRepository _userReceiverRepository;
    public IdentityService(IUserRepository userRepository, ISignInService signInService, IRoleRepository roleRepository, ILogger<IdentityService> logger, IMapper mapper, IUserReceiverRepository userReceiverRepository)
    {
        _userRepository = userRepository;
        _signInService = signInService;
        _roleRepository = roleRepository;
        _logger = logger;
        _mapper = mapper;
        _userReceiverRepository = userReceiverRepository;
    }

    public async Task<Result<bool>> RegisterAsync(UserDomain user)
    {
        var userEntity = _mapper.Map<UserEntity>(user);
        
        var exists = await _userRepository.FindByEmailAsync(user.Email);
        if (exists != null)
        {
            var badResult = new Result<bool>(new RegistrationOperationException(new []{ "User with same email already exists" }));
            return badResult;
        }
        
        // Add user
        var result = await _userRepository.CreateAsync(userEntity, user.Password);
        if (result.Succeeded)
        {
            // Set roles 
            await SetRoleToUserAsync("user", userEntity);
            if (user.IsLandlord == true)
            {
                await SetRoleToUserAsync("landlord", userEntity);
            }

            // Set cookies
            await _signInService.SignInAsync(userEntity, false);
            return new Result<bool>(true);
        }
        else
        {
            return new Result<bool>(new RegistrationOperationException(result.Errors.Select(x => x.Description).ToArray()));
        }
    }
    public async Task<Result<bool>> LoginAsync(string username, string password)
    {
        var result = await _signInService.PasswordSignInAsync(username, password, false, false);

        if (result.Succeeded == true)
        {
            return new Result<bool>(true);
        }
        else
        {
            return new Result<bool>(new LoginOperationException(new[] { "User with this username and password does not exist" }));
        }
    }
    public async Task SignOutAsync()
    {
        await _signInService.SignOutAsync();
    }
    private async Task SetRoleToUserAsync(string roleName, UserEntity user)
    {
        var role = await _roleRepository.FindByNameAsync(roleName);
        
        if (role == null)
        {
            var exception = new NotFoundRoleException($"Can not find role: {roleName}");
            _logger.LogCritical(exception, "Can not find role");
            throw exception;
        }

        IdentityResult roleAddingResult = await _userRepository.AddToRoleAsync(user, role.Name);
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
    public async Task<Result<bool>> UpdateUserFields(Guid userId, string newUsername, string newInformation, string password)
    {
        var entity = await _userReceiverRepository.GetByIdWithContactsAsync(userId);
        if (entity == null)
        {
            var badResult = new Result<bool>(new UserNotFoundException(new[] { "User with given id does not exist" }));
            return badResult;
        }
        
        /* Validate password */
        var givenPasswordSameToActual = await _userRepository.CheckPasswordAsync(entity, password);
        if(givenPasswordSameToActual == false)
        {
            var badResult = new Result<bool>(new WrongPasswordException(new[] { "Given password and actual are not the same" }));
            return badResult;
        }
        
        /* Update user fields */
        var result = await _userRepository.UpdateFieldsAsync(entity, newUsername, newInformation);
        return result.Succeeded ? new Result<bool>(true) : 
            new Result<bool>(new UserUpdateOperationException(result.Errors
                .Select(x => x.Description).ToArray()));
    }
}