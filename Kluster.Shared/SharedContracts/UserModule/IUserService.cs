using ErrorOr;
using Kluster.Shared.DTOs.Requests.User;
using Kluster.Shared.DTOs.Responses.User;

namespace Kluster.Shared.SharedContracts.UserModule;

public interface IUserService
{
    Task<ErrorOr<UserResponse>> GetLoggedInUser();
    Task<ErrorOr<Updated>> UpdateUser(UpdateUserRequest request);

    Task<string> GenerateOtpForEmail(string userId);
    Task<ErrorOr<Success>> ConfirmEmailWithToken(string emailAddress, string otp); // todo: create endpoint
    Task<ErrorOr<Success>> ResendVerificationMessage(string id, string verificationRoute);

    Task<ErrorOr<Success>> SendForgotPasswordMail(ForgotPasswordRequest request);
    Task<ErrorOr<Success>> ResetPassword(ResetPasswordRequest request);
}