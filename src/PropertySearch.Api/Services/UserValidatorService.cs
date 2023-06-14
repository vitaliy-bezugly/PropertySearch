using PropertySearch.Api.Common;
using PropertySearch.Api.Common.Constants;
using PropertySearch.Api.Common.Logging;
using PropertySearch.Api.Repositories.Abstract;
using PropertySearch.Api.Services.Abstract;
using PropertySearch.Api.Common.Extensions;

namespace PropertySearch.Api.Services;

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
        try
        {
            var user = validateAccess ? await _userReceiver.GetByIdWithAccommodationsAsync(userId) :
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
            else if (user.Accommodations != null && validateAccess && user.Accommodations.Any(x => x.Id == accommodationId) == false)
            {
                _logger.LogWarning("Access error");
                return new OperationResult(ErrorMessages.User.HasNoAccess);
            }

            // success
            return new OperationResult();
        }
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(UserValidatorService))
                .WithMethod(nameof(ValidateAsync))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(nameof(Guid), nameof(userId), userId.ToString())
                .WithParameter(nameof(Guid), nameof(accommodationId), accommodationId.ToString())
                .WithParameter(nameof(Boolean), nameof(validateAccess), validateAccess.ToString())
                .ToString());
            
            throw;
        }
    }
}