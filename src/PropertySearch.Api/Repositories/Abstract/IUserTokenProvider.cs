using PropertySearch.Api.Entities;

namespace PropertySearch.Api.Repositories.Abstract;

public interface IUserTokenProvider
{
    public Task<string> GenerateEmailConfirmationTokenAsync(UserEntity user);
}