using Kluster.Shared.API;
using Kluster.Shared.DTOs.Requests.User;
using Kluster.Shared.Extensions;
using Kluster.Shared.SharedContracts.UserModule;
using Microsoft.AspNetCore.Authorization;
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

    [AllowAnonymous]
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailWithToken(string userId, string token)
    {
        var confirmEmailResult = await userService.ConfirmEmailWithToken(userId, token);
        return confirmEmailResult.Match(_ => NoContent(), ReturnErrorResponse);
    }
    
    [AllowAnonymous]
    [HttpPost("{id}/resend-verification-message")]
    // route is email or phone
    public async Task<IActionResult> ResendVerificationMessage(string id, string verificationRoute)
    {
        var generateNewOtpResult = await userService.ResendVerificationMessage(id, verificationRoute);
        return generateNewOtpResult.Match(_ => NoContent(), ReturnErrorResponse);
    }
}