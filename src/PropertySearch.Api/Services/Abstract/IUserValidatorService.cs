using PropertySearch.Api.Common;

namespace PropertySearch.Api.Services.Abstract;

public interface IUserValidatorService
{
    public Task<OperationResult> ValidateAsync(Guid userId, Guid accommodationId, bool validateAccess);
}