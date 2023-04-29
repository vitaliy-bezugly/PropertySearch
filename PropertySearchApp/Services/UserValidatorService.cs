using PropertySearchApp.Common;
using PropertySearchApp.Common.Constants;
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

    public async Task<OperationResult> ValidateAsync(Guid userId, Guid accommodationId, bool validateAccess)
    {
        var user = validateAccess == true ? await _userReceiver.GetByIdWithAccommodationsAsync(userId) :
                await _userReceiver.GetByIdAsync(userId);
        
        if (user == null)
        {
            _logger.LogWarning("Can not validate not existing user");
            return new OperationResult(ErrorMessages.User.NotFound);
        }

        if (user.IsLandlord == false)
        {
            _logger.LogWarning("User is not a landlord");
            return new OperationResult(ErrorMessages.User.NotLandlord);
        }
        else if (validateAccess == true && user.Accommodations.Any(x => x.Id == accommodationId) == false)
        {
            _logger.LogWarning("Access error");
            return new OperationResult(ErrorMessages.User.HasNoAccess);
        }

        // success
        return new OperationResult();
    }
}