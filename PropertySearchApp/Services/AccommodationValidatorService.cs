using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using PropertySearchApp.Common.Exceptions;
using PropertySearchApp.Domain;
using PropertySearchApp.Entities;

namespace PropertySearchApp.Services;

public class AccommodationValidatorService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly ILogger<AccommodationValidatorService> _logger;
    public AccommodationValidatorService(UserManager<UserEntity> userManager, ILogger<AccommodationValidatorService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<bool>> ValidateAsync(AccommodationDomain accommodation)
    {
        var user = await _userManager.FindByIdAsync(accommodation.UserId.ToString());
        if (user == null)
        {
            var exception = new AccommodationDataSourceException(new[] { "There is no user with given id" });
            _logger.LogWarning(exception, "Can not create accommodation");
            return new Result<bool>(exception);
        }
        else if (user.IsLandlord == false)
        {
            var exception = new AccommodationDataSourceException(new[] { "Regular user can not create accommodation's offers" });
            _logger.LogWarning(exception, "User is not a landlord");
            return new Result<bool>(exception);
        }

        return true;
    }
}