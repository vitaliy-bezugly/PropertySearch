using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PropertySearchApp.Common.Exceptions;
using PropertySearchApp.Domain;
using PropertySearchApp.Entities;

namespace PropertySearchApp.Services;

public class UserValidatorService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly ILogger<UserValidatorService> _logger;
    public UserValidatorService(UserManager<UserEntity> userManager, ILogger<UserValidatorService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<bool>> ValidateAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if (user == null)
        {
            var exception = new InvalidOperationException("There is no user with given id");
            _logger.LogError(exception, "Can not validate not existing user");
            throw exception;
        }
        if (user.IsLandlord == false)
        {
            var exception = new UserValidationException(new[] { "Regular user can not create accommodation's offers" });
            _logger.LogWarning(exception, "User is not a landlord");
            return new Result<bool>(exception);
        }

        return true;
    }

    public async Task<Result<bool>> ValidateAccessToAccommodationAsync(Guid userId, Guid accommodationId)
    {
        var user = await _userManager.Users.Include(x => x.Accommodations)
            .FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null)
            throw new ArgumentException($"{nameof(user)} can not be null");
        
        if (user.Accommodations.Any(x => x.Id == accommodationId) == false)
        {
            var exception = new UserValidationException(new[] { "Given user has no access to this accommodation" });
            _logger.LogWarning(exception, "Access error");
            return new Result<bool>(exception);
        }

        return new Result<bool>(true);
    }
}