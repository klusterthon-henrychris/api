using ErrorOr;
using Kluster.Shared.DTOs.Requests.Auth;
using Kluster.Shared.DTOs.Responses.Auth;

namespace Kluster.UserModule.Services.Contracts;

public interface IAuthenticationService
{
    Task<ErrorOr<UserAuthResponse>> RegisterAsync(RegisterRequest request);
    Task<ErrorOr<UserAuthResponse>> LoginAsync(LoginRequest request);
}