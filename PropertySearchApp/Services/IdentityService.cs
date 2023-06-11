using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using PropertySearchApp.Common;
using PropertySearchApp.Common.Constants;
using PropertySearchApp.Common.Exceptions;
using PropertySearchApp.Common.Extensions;
using PropertySearchApp.Common.Logging;
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
    private readonly IUserTokenProvider _tokenProvider;
    private readonly IEmailSender _emailSender;
    private readonly IHtmlMessageBuilder _htmlMessageBuilder;
    public IdentityService(IUserRepository userRepository, ISignInService signInService, IRoleRepository roleRepository, ILogger<IdentityService> logger, IMapper mapper, IUserReceiverRepository userReceiverRepository, IUserTokenProvider tokenProvider, IEmailSender emailSender, IHtmlMessageBuilder htmlMessageBuilder)
    {
        _userRepository = userRepository;
        _signInService = signInService;
        _roleRepository = roleRepository;
        _logger = logger;
        _mapper = mapper;
        _userReceiverRepository = userReceiverRepository;
        _tokenProvider = tokenProvider;
        _emailSender = emailSender;
        _htmlMessageBuilder = htmlMessageBuilder;
    }

    public async Task<OperationResult> RegisterAsync(UserDomain user)
    {
        try
        {
            var exists = await _userRepository.FindByEmailAsync(user.Email);
            if (exists != null)
            {
                return new OperationResult(ErrorMessages.User.SameEmail);
            }

            var userEntity = _mapper.Map<UserEntity>(user);

            var result = await _userRepository.CreateAsync(userEntity, user.Password);
            if (result.Succeeded)
            {
                await SetRolesAsync(userEntity);

                await SendConfirmationEmailAsync(userEntity);

                // Set cookies
                await _signInService.SignInAsync(userEntity, false);
                return new OperationResult();
            }

            return new OperationResult(result.Errors.First().Description);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityService))
                .WithMethod(nameof(RegisterAsync))
                .WithOperation("Post")
                .WithComment(e.Message)
                .WithParameter(typeof(UserDomain).FullName ?? String.Empty, nameof(user), user.SerializeObject())
                .ToString());

            throw;
        }
    }

    public async Task<OperationResult> LoginAsync(string username, string password)
    {
        try
        {
            var result = await _signInService.PasswordSignInAsync(username, password, false, false);
            return result.Succeeded ? new OperationResult()
                : new OperationResult(ErrorMessages.User.WrongCredentials);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityService))
                .WithMethod(nameof(LoginAsync))
                .WithOperation("Post")
                .WithComment(e.Message)
                .WithParameter(nameof(String), nameof(username), username)
                .WithParameter(nameof(String), nameof(password), password)
                .ToString());
            
            throw;
        }
    }
    
    public async Task SignOutAsync()
    {
        try
        {
            await _signInService.SignOutAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityService))
                .WithMethod(nameof(SignOutAsync))
                .WithOperation("Post")
                .WithComment(e.Message)
                .WithNoParameters()
                .ToString());
            
            throw;
        }
    }
    
    private async Task SetRoleToUserAsync(string roleName, UserEntity user)
    {
        try
        {
            var role = await _roleRepository.FindByNameAsync(roleName);
            if (role is null)
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
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityService))
                .WithMethod(nameof(SetRoleToUserAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(nameof(String), nameof(roleName), roleName)
                .WithParameter(typeof(UserEntity).FullName ?? String.Empty, nameof(user), user.SerializeObject())
                .ToString());

            throw;
        }
    }
    
    public async Task<UserDomain?> GetUserByIdAsync(Guid userId)
    {
        try
        {
            var entity = await _userReceiverRepository.GetByIdAsync(userId);
            return entity == null ? null : _mapper.Map<UserDomain>(entity);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityService))
                .WithMethod(nameof(GetUserByIdAsync))
                .WithOperation("Get")
                .WithComment(e.Message)
                .WithParameter(nameof(Guid), nameof(userId), userId.ToString())
                .ToString());
            
            throw;
        }
    }
    
    public async Task<OperationResult> UpdateUserFieldsAsync(Guid userId, string newUsername, string newInformation, string password)
    {
        try
        {
            var entity = await _userReceiverRepository.GetByIdWithContactsAsync(userId);
            if (entity == null)
            {
                return new OperationResult(ErrorMessages.User.NotFound);
            }
        
            /* Validate password */
            var operationResult = await CheckPasswordAsync(password, entity);
            if (operationResult.Succeeded == false)
                return operationResult;

            /* Update user fields */
            var result = await _userRepository.UpdateFieldsAsync(entity, newUsername, newInformation);
            if (result.Succeeded)
                return new OperationResult();

            return HandleErrors(result.Errors, "Can not update user fields.");
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityService))
                .WithMethod(nameof(UpdateUserFieldsAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(nameof(Guid), nameof(userId), userId.ToString())
                .WithParameter(nameof(String), nameof(newUsername), newUsername)
                .WithParameter(nameof(String), nameof(newInformation), newInformation)
                .WithParameter(nameof(String), nameof(password), password)
                .ToString());
            
            throw;
        }
    }

    public async Task<OperationResult> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        try
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
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityService))
                .WithMethod(nameof(ChangePasswordAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(nameof(Guid), nameof(userId), userId.ToString())
                .WithParameter(nameof(String), nameof(currentPassword), currentPassword)
                .WithParameter(nameof(String), nameof(newPassword), newPassword)
                .ToString());
            
            throw;
        }
    }

    public async Task<OperationResult> ConfirmEmailAsync(Guid userId, string token)
    {
        try
        {
            UserEntity? user = await _userReceiverRepository.GetByIdAsync(userId);
            if (user is null)
                return new OperationResult(ErrorMessages.User.NotFound);

            token = token.Replace(' ', '+');
            IdentityResult result = await _userRepository.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
                return new OperationResult();
            
            _logger.LogWarning($"Can not confirm email with token: {token}");
            return HandleErrors(result.Errors, "Something goes wrong while email confirmation");
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityService))
                .WithMethod(nameof(ConfirmEmailAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(nameof(Guid), nameof(userId), userId.ToString())
                .WithParameter(nameof(String), nameof(token), token)
                .ToString());
            
            throw;
        }
    }

    public async Task<bool> IsEmailConfirmedAsync(Guid userId)
    {
        try
        {
            var user = await _userReceiverRepository.GetByIdAsync(userId);
            if (user is null)
                throw new UserNotFoundException();

            return user.EmailConfirmed;
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityService))
                .WithMethod(nameof(IsEmailConfirmedAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(nameof(Guid), nameof(userId), userId.ToString())
                .ToString());
            
            throw;
        }
    }

    public async Task SendConfirmationEmailAsync(Guid userId)
    {
        var user = await _userReceiverRepository.GetByIdAsync(userId);
        if (user is null)
            throw new UserNotFoundException();

        await SendConfirmationEmailAsync(user);
    }
    
    private async Task SendConfirmationEmailAsync(UserEntity user)
    {
        var token = await _tokenProvider.GenerateEmailConfirmationTokenAsync(user);
        if (string.IsNullOrEmpty(token) == false)
        {
            string emailContent = _htmlMessageBuilder.BuildEmailConfirmationMessage(user.Id, user.UserName, token);
            await _emailSender.SendEmailAsync(user.Email, "Email confirmation", emailContent);
        }
        else
        {
            throw new InvalidOperationException("For some reason we can not create confirmation token");
        }
    }

    private OperationResult HandleErrors(IEnumerable<IdentityError> errors, string errorMessageForLogger)
    {
        _logger.LogWarning(errorMessageForLogger);
        var identityErrors = errors.ToList();
        foreach (var error in identityErrors)
        {
            _logger.LogWarning($"Code: {error.Code}; Description: {error.Description}");
        }
        return new OperationResult(identityErrors.First().Description);
    }
    
    private async Task SetRolesAsync(UserEntity userEntity)
    {
        await SetRoleToUserAsync("user", userEntity);
        if (userEntity.IsLandlord)
        {
            await SetRoleToUserAsync("landlord", userEntity);
        }
    }
    
    private async Task<OperationResult> CheckPasswordAsync(string password, UserEntity entity)
    {
        var givenPasswordSameToActual = await _userRepository.CheckPasswordAsync(entity, password);
        if (givenPasswordSameToActual == false)
        {
            return new OperationResult(ErrorMessages.User.WrongPassword);
        }

        return new OperationResult();
    }
}