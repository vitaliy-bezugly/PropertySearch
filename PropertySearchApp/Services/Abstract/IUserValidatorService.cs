using LanguageExt.Common;

namespace PropertySearchApp.Services.Abstract;

public interface IUserValidatorService
{
    public Task<Result<bool>> ValidateAsync(Guid userId);
    public Task<Result<bool>> ValidateAccessToAccommodationAsync(Guid userId, Guid accommodationId);
}