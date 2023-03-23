using LanguageExt.Common;

namespace PropertySearchApp.Services.Abstract;

public interface IUserValidatorService
{
    public Task<Result<bool>> ValidateAsync(Guid userId, Guid accommodationId, bool validateAccess);
}