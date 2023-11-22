using ErrorOr;
using Kluster.UserModule.DTOs.Requests;
using Kluster.UserModule.DTOs.Responses;

namespace Kluster.UserModule.Services.Contracts;

public interface IUserService
{
    Task<ErrorOr<UserResponse>> GetLoggedInUser();
    Task<ErrorOr<Updated>> UpdateUser(UpdateUserRequest request);
}