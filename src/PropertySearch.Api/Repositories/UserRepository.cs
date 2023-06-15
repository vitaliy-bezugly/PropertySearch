using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PropertySearch.Api.Entities;
using PropertySearch.Api.Repositories.Abstract;
using PropertySearch.Api.Persistence;

namespace PropertySearch.Api.Repositories;

public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
{
    private readonly UserManager<UserEntity> _userManager;
    public UserRepository(UserManager<UserEntity> userManager, ApplicationDbContext context) 
        : base(context)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> AddToRoleAsync(UserEntity user, string roleName)
    {
        return await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task<bool> CheckPasswordAsync(UserEntity user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IdentityResult> CreateAsync(UserEntity user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<UserEntity?> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id)
    {
        return await Table.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<UserEntity?> GetByIdWithAccommodationsAsync(Guid userId)
    {
        return await Table
            .Include(x => x.Accommodations)
            .FirstOrDefaultAsync(x => x.Id == userId);
    }
    
    public async Task<UserEntity?> GetByIdWithContactsAsync(Guid userId)
    {
        return await Table
            .Include(x => x.Contacts)
            .FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<IdentityResult> ConfirmEmailAsync(UserEntity user, string token)
    {
        var identityResult = await _userManager.ConfirmEmailAsync(user, token);
        return identityResult;
    }

    public async Task<IdentityResult> ChangePasswordAsync(UserEntity user, string currentPassword, string newPassword)
    {
        return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }
}
