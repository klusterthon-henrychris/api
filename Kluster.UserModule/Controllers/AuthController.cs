using System.ComponentModel.DataAnnotations;
using Kluster.Shared.API;
using Kluster.Shared.Extensions;
using Kluster.UserModule.DTOs.Requests;
using Kluster.UserModule.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kluster.UserModule.Controllers;

public class AuthController(IAuthenticationService authenticationService) : BaseController
{
    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync([Required] RegisterRequest request)
    {
        var registerResult = await authenticationService.RegisterAsync(request);
        return registerResult.Match(_ => Ok(registerResult.ToSuccessfulApiResponse()), ReturnErrorResponse);
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([Required] LoginRequest request)
    {
        var loginResult = await authenticationService.LoginAsync(request);
        return loginResult.Match(
            _ => Ok(loginResult.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }
}