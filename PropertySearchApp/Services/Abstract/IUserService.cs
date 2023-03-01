using LanguageExt.Common;

namespace PropertySearchApp.Services.Abstract;

public interface IUserService
{
    Task<Result<bool>> RegisterAsync(string username, string email, string password);
    Task<Result<bool>> LoginAsync(string email, string password);
    Task SignOutAsync();
}