using ErrorOr;
using Kluster.UserModule.DTOs.Responses;

namespace Kluster.UserModule.Services.Contracts;

public interface IUserService
{
    Task<ErrorOr<UserResponse>> GetLoggedInUser();
}