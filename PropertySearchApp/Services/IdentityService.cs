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

                await BuildTokenAndSendConfirmationEmailAsync(userEntity);

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
                .WithParameter(typeof(UserDomain).FullName, nameof(user), user.SerializeObject())
                .ToString());
            
            throw;
        }
    }

    public async Task<OperationResult> LoginAsync(string username, string password)
    {
        try
        {
            var result = await _signInService.PasswordSignInAsync(username, password, false, false);
            return result.Succeeded == true ? new OperationResult()
                : new OperationResult(ErrorMessages.User.WrongCredentials);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityService))
                .WithMethod(nameof(LoginAsync))
                .WithOperation("Post")
                .WithComment(e.Message)
                .WithParameter(typeof(string).Name, nameof(username), username)
                .WithParameter(typeof(string).Name, nameof(password), password)
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
                .WithParameter(typeof(string).Name, nameof(roleName), roleName)
                .WithParameter(typeof(UserEntity).FullName, nameof(user), user.SerializeObject())
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
                .WithParameter(typeof(Guid).Name, nameof(userId), userId.ToString())
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
                .WithParameter(typeof(Guid).Name, nameof(userId), userId.ToString())
                .WithParameter(typeof(string).Name, nameof(newUsername), newUsername)
                .WithParameter(typeof(string).Name, nameof(newInformation), newInformation)
                .WithParameter(typeof(string).Name, nameof(password), password)
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
                .WithParameter(typeof(Guid).Name, nameof(userId), userId.ToString())
                .WithParameter(typeof(string).Name, nameof(currentPassword), currentPassword)
                .WithParameter(typeof(string).Name, nameof(newPassword), newPassword)
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
        
            IdentityResult result = await _userRepository.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
                return new OperationResult();

            return HandleErrors(result.Errors, "Something goes wrong while email confirmation");
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityService))
                .WithMethod(nameof(ConfirmEmailAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(typeof(Guid).Name, nameof(userId), userId.ToString())
                .WithParameter(typeof(string).Name, nameof(token), token)
                .ToString());
            
            throw;
        }
    }

    private OperationResult HandleErrors(IEnumerable<IdentityError> errors, string errorMessageForLogger)
    {
        try
        {
            _logger.LogWarning(errorMessageForLogger);
            foreach (var error in errors)
            {
                _logger.LogWarning($"Code: {error.Code}; Description: {error.Description}");
            }
            return new OperationResult(errors.First().Description);
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityService))
                .WithMethod(nameof(HandleErrors))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(typeof(IEnumerable<IdentityError>).FullName, nameof(errors), errors.SerializeObject())
                .WithParameter(typeof(string).Name, nameof(errorMessageForLogger), errorMessageForLogger)
                .ToString());
            
            throw;
        }
    }
    
    private async Task SetRolesAsync(UserEntity userEntity)
    {
        try
        {
            await SetRoleToUserAsync("user", userEntity);
            if (userEntity.IsLandlord == true)
            {
                await SetRoleToUserAsync("landlord", userEntity);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityService))
                .WithMethod(nameof(SetRolesAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(typeof(UserEntity).FullName, nameof(userEntity), userEntity.SerializeObject())
                .ToString());
            
            throw;
        }
    }
    
    private async Task<OperationResult> CheckPasswordAsync(string password, UserEntity entity)
    {
        try
        {
            var givenPasswordSameToActual = await _userRepository.CheckPasswordAsync(entity, password);
            if (givenPasswordSameToActual == false)
            {
                return new OperationResult(ErrorMessages.User.WrongPassword);
            }

            return new OperationResult();
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IdentityService))
                .WithMethod(nameof(CheckPasswordAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(typeof(string).Name, nameof(password), password)
                .WithParameter(typeof(UserEntity).FullName, nameof(entity), entity.SerializeObject())
                .ToString());
            
            throw;
        }
    }
    
    private async Task BuildTokenAndSendConfirmationEmailAsync(UserEntity user)
    {
        var token = await _tokenProvider.GenerateEmailConfirmationTokenAsync(user);
        if (string.IsNullOrEmpty(token) == false)
        {
            string emailContent = _htmlMessageBuilder.BuildEmailConfirmationMessage(user.Id, user.UserName, token);
            await _emailSender.SendEmailAsync(user.Email, "Email confirmation", emailContent);
        }
    }
}