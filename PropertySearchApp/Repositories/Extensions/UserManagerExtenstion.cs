using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PropertySearchApp.Common.Exceptions;
using PropertySearchApp.Entities;

namespace PropertySearchApp.Repositories.Extensions;

public static class UserManagerExtenstion
{
    public static async Task<Result<bool>> UpdateUserFieldsAsync(this UserManager<UserEntity> userManager, UserEntity user)
    {
        var toUpdate = await userManager.Users.Include(x => x.Contacts).FirstOrDefaultAsync(x => x.Id == user.Id);
        if(toUpdate == null)
        {
            var badResult = new Result<bool>(new UserNotFoundException(new[] { "User with given id does not exist" }));
            return badResult;
        }

        toUpdate.UserName = user.UserName;
        toUpdate.Information = user.Information;
        toUpdate.Contacts = user.Contacts;

        var result = await userManager.UpdateAsync(toUpdate);
        if (result.Succeeded)
        {
            return new Result<bool>(true);
        }
        else
        {
            return new Result<bool>(new UserUpdateOperationException(result.Errors.Select(x => x.Description).ToArray()));
        }
    }
}
