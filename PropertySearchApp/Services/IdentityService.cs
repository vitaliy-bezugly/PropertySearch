using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PropertySearchApp.Common;
using PropertySearchApp.Common.Constants;
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

    public async Task<OperationResult> RegisterAsync(UserDomain user)
    {
        var userEntity = _mapper.Map<UserEntity>(user);
        
        var exists = await _userRepository.FindByEmailAsync(user.Email);
        if (exists != null)
        {
            return new OperationResult(ErrorMessages.User.SameEmail);
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
            return new OperationResult();
        }

        return new OperationResult(result.Errors.First().Description);
    }
    public async Task<OperationResult> LoginAsync(string username, string password)
    {
        var result = await _signInService.PasswordSignInAsync(username, password, false, false);
        return result.Succeeded == true ? new OperationResult()
            : new OperationResult(ErrorMessages.User.WrongCredentials);
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
    public async Task<OperationResult> UpdateUserFieldsAsync(Guid userId, string newUsername, string newInformation, string password)
    {
        var entity = await _userReceiverRepository.GetByIdWithContactsAsync(userId);
        if (entity == null)
        {
            return new OperationResult(ErrorMessages.User.NotFound);
        }
        
        /* Validate password */
        var givenPasswordSameToActual = await _userRepository.CheckPasswordAsync(entity, password);
        if(givenPasswordSameToActual == false)
        {
            return new OperationResult(ErrorMessages.User.WrongPassword);
        }
        
        /* Update user fields */
        var result = await _userRepository.UpdateFieldsAsync(entity, newUsername, newInformation);
        if (result.Succeeded)
            return new OperationResult();

        return HandleErrors(result.Errors, "Can not update user fields.");
    }
    public async Task<OperationResult> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        var user = await _userReceiverRepository.GetByIdAsync(userId);
        if(user is null)
        {
            return new OperationResult(ErrorMessages.User.NotFound);
        }

        var result = await _userRepository.ChangePasswordAsync(user, currentPassword, newPassword); 
        if(result.Succeeded)
            return new OperationResult();

        return HandleErrors(result.Errors, "Can not change password");
    }

    private OperationResult HandleErrors(IEnumerable<IdentityError> errors, string errorMessageForLogger)
    {
        _logger.LogWarning(errorMessageForLogger);
        foreach (var error in errors)
        {
            _logger.LogWarning($"Code: {error.Code}; Description: {error.Description}");
        }
        return new OperationResult(errors.First().Description);
    }
}