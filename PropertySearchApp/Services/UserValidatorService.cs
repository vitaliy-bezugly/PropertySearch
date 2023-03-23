using LanguageExt.Common;
using PropertySearchApp.Common.Exceptions;
using PropertySearchApp.Repositories.Abstract;
using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Services;

public class UserValidatorService : IUserValidatorService
{
    private readonly IUserReceiverRepository _userReceiver;
    private readonly ILogger<UserValidatorService> _logger;
    public UserValidatorService(IUserReceiverRepository userReceiver, ILogger<UserValidatorService> logger)
    {
        _userReceiver = userReceiver;
        _logger = logger;
    }

    public async Task<Result<bool>> ValidateAsync(Guid userId)
    {
        var user = await _userReceiver.GetByIdAsync(userId);
        
        if (user == null)
        {
            var exception = new UserNotFoundException(new string[] { "There is no user with given id" });
            _logger.LogWarning(exception, "Can not validate not existing user");
            return new Result<bool>(exception);
        }
        else if (user.IsLandlord == false)
        {
            var exception = new UserValidationException(new[] { "Regular user can not create accommodation's offers" });
            _logger.LogWarning(exception, "User is not a landlord");
            return new Result<bool>(exception);
        }

        return true;
    }

    public async Task<Result<bool>> ValidateAccessToAccommodationAsync(Guid userId, Guid accommodationId)
    {
        var user = await _userReceiver.GetByIdWithAccommodationsAsync(userId);
        
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