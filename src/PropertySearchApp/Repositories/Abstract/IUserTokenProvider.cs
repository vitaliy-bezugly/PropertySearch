using PropertySearchApp.Entities;

namespace PropertySearchApp.Repositories.Abstract;

public interface IUserTokenProvider
{
    public Task<string> GenerateEmailConfirmationTokenAsync(UserEntity user);
}