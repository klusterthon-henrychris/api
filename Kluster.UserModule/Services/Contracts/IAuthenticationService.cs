using ErrorOr;
using Kluster.UserModule.DTOs.Requests;
using Kluster.UserModule.DTOs.Responses;

namespace Kluster.UserModule.Services.Contracts;

public interface IAuthenticationService
{
    Task<ErrorOr<UserAuthResponse>> RegisterAsync(RegisterRequest request);
    Task<ErrorOr<UserAuthResponse>> LoginAsync(LoginRequest request);
}