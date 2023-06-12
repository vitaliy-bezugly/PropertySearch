using PropertySearch.Api.Entities;

namespace PropertySearch.Api.Repositories.Abstract;

public interface IUserReceiverRepository
{
    Task<UserEntity?> GetByIdAsync(Guid id);
    Task<UserEntity?> GetByIdWithAccommodationsAsync(Guid userId);
    Task<UserEntity?> GetByIdWithContactsAsync(Guid userId);
}