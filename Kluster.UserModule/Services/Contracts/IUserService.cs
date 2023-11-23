using ErrorOr;
using Kluster.Shared.DTOs.Requests.User;
using Kluster.Shared.DTOs.Responses.User;

namespace Kluster.UserModule.Services.Contracts;

public interface IUserService
{
    Task<ErrorOr<UserResponse>> GetLoggedInUser();
    Task<ErrorOr<Updated>> UpdateUser(UpdateUserRequest request);
}