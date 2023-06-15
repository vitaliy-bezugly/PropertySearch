using PropertySearch.Api.Common;
using PropertySearch.Api.Common.Constants;
using PropertySearch.Api.Services.Abstract;
using PropertySearch.Api.Persistence;

namespace PropertySearch.Api.Services;

public class UserValidatorService : IUserValidatorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserValidatorService> _logger;
    public UserValidatorService(IUnitOfWork unitOfWork, ILogger<UserValidatorService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<OperationResult> ValidateAsync(Guid userId, Guid accommodationId, bool validateAccess)
    {
        var user = await _unitOfWork.UserRepository.GetByIdWithAccommodationsAsync(userId);
        
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
            
        if (user.Accommodations is not null && validateAccess && user.Accommodations.Any(x => x.Id == accommodationId) == false)
        {
            _logger.LogWarning("Access error");
            return new OperationResult(ErrorMessages.User.HasNoAccess);
        }

        return OperationResult.Success;
    }
}