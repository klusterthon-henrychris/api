using ErrorOr;
using Kluster.Shared.Domain;
using Kluster.Shared.Exceptions;
using Kluster.Shared.ServiceErrors;
using Kluster.Shared.SharedContracts.UserModule;
using Kluster.UserModule.Data;
using Kluster.UserModule.DTOs.Responses;
using Kluster.UserModule.ServiceErrors;
using Kluster.UserModule.Services.Contracts;

namespace Kluster.UserModule.Services;

public class UserService(ICurrentUser currentUser, UserModuleDbContext context) : IUserService
{
    public async Task<ErrorOr<UserResponse>> GetLoggedInUser()
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException("");
        var user = await context.Users.FindAsync(userId);
        if (user is null)
        {
            return Errors.User.NotFound;
        }

        return UserModuleMapper.ToUserResponse(user);
    }
}