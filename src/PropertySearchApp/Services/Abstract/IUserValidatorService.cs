using PropertySearchApp.Common;

namespace PropertySearchApp.Services.Abstract;

public interface IUserValidatorService
{
    public Task<OperationResult> ValidateAsync(Guid userId, Guid accommodationId, bool validateAccess);
}