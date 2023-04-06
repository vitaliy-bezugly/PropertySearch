using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PropertySearchApp.Entities;
using PropertySearchApp.Repositories.Abstract;

namespace PropertySearchApp.Repositories;

public class UserReceiverRepository : IUserReceiverRepository
{
    private readonly UserManager<UserEntity> _userManager;
    public UserReceiverRepository(UserManager<UserEntity> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id)
    {
        return await _userManager.Users
            .Include(x => x.Contacts)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<UserEntity?> GetByIdWithAccommodationsAsync(Guid userId)
    {
        return await _userManager.Users.Include(x => x.Accommodations).FirstOrDefaultAsync(x => x.Id == userId);
    }
}