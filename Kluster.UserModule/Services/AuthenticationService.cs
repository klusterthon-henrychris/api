using System.Text.Json;
using ErrorOr;
using Kluster.Shared.Domain;
using Kluster.Shared.Extensions;
using Kluster.UserModule.DTOs.Requests;
using Kluster.UserModule.DTOs.Responses;
using Kluster.UserModule.ServiceErrors;
using Kluster.UserModule.Services.Contracts;
using Kluster.UserModule.Validators;
using Microsoft.AspNetCore.Identity;


namespace Kluster.UserModule.Services;

public class AuthenticationService(
    ITokenService tokenService,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ILogger<AuthenticationService> logger)
    : IAuthenticationService
{
    public async Task<ErrorOr<UserAuthResponse>> RegisterAsync(RegisterRequest request)
    {
        var validateResult = await new RegisterRequestValidator().ValidateAsync(request);
        if (!validateResult.IsValid)
        {
            return validateResult.ToErrorList();
        }

        var user = await userManager.FindByEmailAsync(request.EmailAddress);
        if (user is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        var newUser = MapToApplicationUser(request);
        var result = await userManager.CreateAsync(newUser, request.Password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, newUser.Role);
            return new UserAuthResponse(Id: newUser.Id,
                FirstName: newUser.FirstName,
                LastName: newUser.LastName,
                EmailAddress: newUser.Email!,
                Role: newUser.Role,
                AccessToken: tokenService.CreateUserJwt(newUser.Email!, newUser.Role, newUser.Id));
        }

        var errors = result.Errors
            .Select(error => Error.Validation("User." + error.Code, error.Description))
            .ToList();

        return errors;
    }

    public async Task<ErrorOr<UserAuthResponse>> LoginAsync(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.EmailAddress);
        if (user is null)
        {
            return Errors.User.NotFound;
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (signInResult.Succeeded)
        {
            logger.LogInformation($"User {user.Id} logged in successfully.");
            return new UserAuthResponse(Id: user.Id,
                FirstName: user.FirstName,
                LastName: user.LastName,
                EmailAddress: user.Email!,
                Role: user.Role,
                AccessToken: tokenService.CreateUserJwt(user.Email!, user.Role, user.Id));
        }

        if (signInResult.IsLockedOut)
        {
            logger.LogInformation($"User {user.Id} is locked out. End date: {user.LockoutEnd}." +
                                  $"\n\tRequest: {JsonSerializer.Serialize(request)}");
            return Errors.User.IsLockedOut;
        }

        if (signInResult.IsNotAllowed)
        {
            logger.LogInformation($"User {user.Id} is not allowed to access the system out." +
                                  $"\n\tRequest: {JsonSerializer.Serialize(request)}");
            return Errors.User.IsNotAllowed;
        }

        logger.LogError($"Login failed for user {user.Id}.\n\tRequest: {JsonSerializer.Serialize(request)}");
        return Errors.Auth.LoginFailed;
    }

    private static ApplicationUser MapToApplicationUser(RegisterRequest request)
    {
        return new ApplicationUser
        {
            FirstName = request.FirstName.FirstCharToUpper(),
            LastName = request.LastName.FirstCharToUpper(),
            Email = request.EmailAddress,
            UserName = request.EmailAddress,
            Address = request.Address,
            Role = request.Role
        };
    }
    // todo: add method to create business in business controller
}