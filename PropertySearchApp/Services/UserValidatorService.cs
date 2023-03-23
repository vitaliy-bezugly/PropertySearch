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

    public async Task<Result<bool>> ValidateAsync(Guid userId, Guid accommodationId, bool validateAccess)
    {
        var user = validateAccess == true ? await _userReceiver.GetByIdWithAccommodationsAsync(userId) :
                await _userReceiver.GetByIdAsync(userId);
        
        if (user == null)
        {
            var exception = new UserNotFoundException(new string[] { "There is no user with given id" });
            _logger.LogWarning(exception, "Can not validate not existing user");
            return new Result<bool>(exception);
        }
        else if (user.IsLandlord == false)
        {
            var exception = new UserValidationException(new[] { "Regular user does not have access to accommodation offers" });
            _logger.LogWarning(exception, "User is not a landlord");
            return new Result<bool>(exception);
        }
        else if (validateAccess == true && user.Accommodations.Any(x => x.Id == accommodationId) == false)
        {
            var exception = new UserValidationException(new[] { "Given user has no access to this accommodation" });
            _logger.LogWarning(exception, "Access error");
            return new Result<bool>(exception);
        }
        else
        {
            return new Result<bool>(true);
        }
    }
}