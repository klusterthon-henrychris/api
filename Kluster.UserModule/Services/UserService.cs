using ErrorOr;
using Kluster.Shared.DTOs.Requests.User;
using Kluster.Shared.DTOs.Responses.User;
using Kluster.Shared.Exceptions;
using Kluster.Shared.Extensions;
using Kluster.Shared.SharedContracts.UserModule;
using Kluster.UserModule.Data;
using Kluster.UserModule.ServiceErrors;
using Kluster.UserModule.Services.Contracts;
using Kluster.UserModule.Validators;

namespace Kluster.UserModule.Services;

public class UserService(ICurrentUser currentUser, UserModuleDbContext context) : IUserService
{
    public async Task<ErrorOr<UserResponse>> GetLoggedInUser()
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        var user = await context.Users.FindAsync(userId);
        if (user is null)
        {
            return Errors.User.NotFound;
        }

        return UserModuleMapper.ToUserResponse(user);
    }

    public async Task<ErrorOr<Updated>> UpdateUser(UpdateUserRequest request)
    {
        var validateResult = await new UpdateUserRequestValidator().ValidateAsync(request);
        if (!validateResult.IsValid)
        {
            return validateResult.ToErrorList();
        }

        var userId = currentUser.UserId ?? throw new UserNotSetException();
        var user = await context.Users.FindAsync(userId);
        if (user is null)
        {
            return Errors.User.NotFound;
        }

        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;
        user.Address = request.Address ?? user.Address;

        context.Update(user);
        await context.SaveChangesAsync();
        return Result.Updated;
    }
}