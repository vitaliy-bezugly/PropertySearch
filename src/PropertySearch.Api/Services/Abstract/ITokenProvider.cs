using PropertySearch.Api.Entities;

namespace PropertySearch.Api.Services.Abstract;

public interface ITokenProvider
{
    Task<string> GenerateEmailConfirmationTokenAsync(UserEntity user);
}