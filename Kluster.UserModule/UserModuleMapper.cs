using Kluster.Shared.Domain;
using Kluster.UserModule.DTOs.Responses;

namespace Kluster.UserModule;

public static class UserModuleMapper
{
    public static UserResponse ToUserResponse(ApplicationUser user)
    {
        return new UserResponse(user.FirstName, user.LastName, user.Email!, user.Address, user.Role);
    }
}