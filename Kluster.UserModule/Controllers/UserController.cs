using Kluster.Shared.API;
using Kluster.Shared.Constants;
using Kluster.Shared.DTOs.Requests.User;
using Kluster.Shared.Extensions;
using Kluster.Shared.SharedContracts.UserModule;
using Kluster.UserModule.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Kluster.UserModule.Controllers;

public class UserController(IUserService userService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetUser()
    {
        var getUserResult = await userService.GetLoggedInUser();

        // If successful, return the event data in an ApiResponse.
        // If an error occurs, return an error response using the ReturnErrorResponse method.
        return getUserResult.Match(
            _ => Ok(getUserResult.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateUserRequest request)
    {
        var updateUserResult = await userService.UpdateUser(request);
        return updateUserResult.Match(_ => NoContent(), ReturnErrorResponse);
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmailWithOtp(string userId, string otp)
    {
        var confirmEmailResult = await userService.ConfirmEmailWithOtp(userId, otp);
        return confirmEmailResult.Match(_ => Ok(), ReturnErrorResponse);
    }

    [HttpPost("/{id}/new-otp")]
    // route is email or phone
    public async Task<IActionResult> GenerateNewOtp(string id, string otpRoute)
    {
        var generateNewOtpResult = await userService.GenerateNewOtp(id, otpRoute);
        return generateNewOtpResult.Match(_ => Ok(), ReturnErrorResponse);
    }
}