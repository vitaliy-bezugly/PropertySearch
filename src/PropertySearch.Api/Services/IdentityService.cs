using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using PropertySearch.Api.Common;
using PropertySearch.Api.Common.Constants;
using PropertySearch.Api.Common.Exceptions;
using PropertySearch.Api.Common.Logging;
using PropertySearch.Api.Domain;
using PropertySearch.Api.Entities;
using PropertySearch.Api.Persistence.Exceptions;
using PropertySearch.Api.Services.Abstract;
using PropertySearch.Api.Common.Extensions;
using PropertySearch.Api.Persistence;

namespace PropertySearch.Api.Services;

public class IdentityService : IIdentityService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISignInService _signInService;
    private readonly ILogger<IdentityService> _logger;
    private readonly ITokenProvider _tokenProvider;
    private readonly IEmailSender _emailSender;
    private readonly IHtmlMessageBuilder _htmlMessageBuilder;
    public IdentityService(IUnitOfWork unitOfWork, ISignInService signInService, ILogger<IdentityService> logger, IMapper mapper, ITokenProvider tokenProvider, IEmailSender emailSender, IHtmlMessageBuilder htmlMessageBuilder)
    {
        _unitOfWork = unitOfWork;
        _signInService = signInService;
        _logger = logger;
        _mapper = mapper;
        _tokenProvider = tokenProvider;
        _emailSender = emailSender;
        _htmlMessageBuilder = htmlMessageBuilder;
    }

    public async Task<OperationResult> RegisterAsync(UserDomain user, CancellationToken cancellationToken)
    {
        try
        {
            var exists = await _unitOfWork.UserRepository.FindByEmailAsync(user.Email);
            if (exists != null)
            {
                return new OperationResult(ErrorMessages.User.SameEmail);
            }

            var userEntity = _mapper.Map<UserEntity>(user);
            var result = await _unitOfWork.UserRepository.CreateAsync(userEntity, user.Password);
            if (result.Succeeded == false)
            {
                return new OperationResult(result.Errors.First().Description);
            }
            
            await SetRolesAsync(userEntity);
            await SendConfirmationEmailAsync(userEntity);

            // Set cookies
            await _signInService.SignInAsync(userEntity, false);
            await _unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Success;
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
        await _signInService.SignOutAsync();
    }

    public async Task<UserDomain?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
        return entity == null ? null : _mapper.Map<UserDomain>(entity);
    }
    
    public async Task<OperationResult> UpdateUserFieldsAsync(Guid userId, string newUsername, 
        string newInformation, string password, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _unitOfWork.UserRepository.GetByIdWithContactsAsync(userId);
            if (entity == null)
            {
                return new OperationResult(ErrorMessages.User.NotFound);
            }
        
            var operationResult = await CheckPasswordAsync(password, entity);
            if (operationResult.Succeeded == false)
                return operationResult;

            entity.UserName = newUsername;
            entity.Information = newInformation;
            await _unitOfWork.UserRepository.UpdateAsync(entity, cancellationToken);
            
            await _unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Success;
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

    public async Task<OperationResult> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
            if(user is null)
            {
                return new OperationResult(ErrorMessages.User.NotFound);
            }

            var result = await _unitOfWork.UserRepository.ChangePasswordAsync(user, currentPassword, newPassword); 
            if(result.Succeeded)
                return OperationResult.Success;

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

    public async Task<OperationResult> ConfirmEmailAsync(Guid userId, string token, CancellationToken cancellationToken)
    {
        try
        {
            UserEntity? user = await _unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
                return new OperationResult(ErrorMessages.User.NotFound);

            token = token.Replace(' ', '+');
            IdentityResult result = await _unitOfWork.UserRepository.ConfirmEmailAsync(user, token);

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

    public async Task<bool> IsEmailConfirmedAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
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

    public async Task SendConfirmationEmailAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
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
        var givenPasswordSameToActual = await _unitOfWork.UserRepository.CheckPasswordAsync(entity, password);
        if (givenPasswordSameToActual == false)
        {
            return new OperationResult(ErrorMessages.User.WrongPassword);
        }

        return OperationResult.Success;
    }
    
    private async Task SetRoleToUserAsync(string roleName, UserEntity user)
    {
        var role = await _unitOfWork.RoleRepository.FindByNameAsync(roleName);
        if (role is null)
        {
            var exception = new NotFoundRoleException($"Can not find role: {roleName}");
            _logger.LogCritical(exception, "Can not find role");
            throw exception;
        }

        IdentityResult roleAddingResult = await _unitOfWork.UserRepository.AddToRoleAsync(user, role.Name);
        if(roleAddingResult.Succeeded == false)
        {
            var exception = new RegistrationOperationException(roleAddingResult.Errors.Select(x => x.Description).ToArray());
            _logger.LogCritical(exception, "Can not add role to user");
            throw exception;
        }
    }
}