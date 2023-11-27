using System.Text.Json;
using ErrorOr;
using Kluster.Shared.Domain;
using Kluster.Shared.DTOs.Requests.Auth;
using Kluster.Shared.DTOs.Responses.Auth;
using Kluster.Shared.Extensions;
using Kluster.Shared.MessagingContracts.Events.User;
using Kluster.UserModule.ServiceErrors;
using Kluster.UserModule.Services.Contracts;
using Kluster.UserModule.Validators;
using MassTransit;
using Microsoft.AspNetCore.Identity;


namespace Kluster.UserModule.Services;

public class AuthenticationService(
    ITokenService tokenService,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IBus bus,
    ILogger<AuthenticationService> logger)
    : IAuthenticationService
{
    public async Task<ErrorOr<UserAuthResponse>> RegisterAsync(RegisterRequest request)
    {
        logger.LogInformation("Registration request received for email: {0}.", request.EmailAddress);
        var validateResult = await new RegisterRequestValidator().ValidateAsync(request);
        if (!validateResult.IsValid)
        {
            return validateResult.ToErrorList();
        }

        var user = await userManager.FindByEmailAsync(request.EmailAddress);
        if (user is not null)
        {
            logger.LogWarning("Duplicate email found during registration: {0}", request.EmailAddress);
            return Errors.User.DuplicateEmail;
        }

        var newUser = MapToApplicationUser(request);
        var result = await userManager.CreateAsync(newUser, request.Password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, newUser.Role);
            await bus.Publish(new EmailOtpRequestedEvent(newUser.FirstName, newUser.LastName, newUser.Email!,
                newUser.Id));

            logger.LogInformation("User registered successfully: {0}.", request.EmailAddress);
            return new UserAuthResponse(Id: newUser.Id,
                Role: newUser.Role,
                AccessToken: tokenService.CreateUserJwt(newUser.Email!, newUser.Role, newUser.Id));
        }

        var errors = result.Errors
            .Select(error => Error.Validation("User." + error.Code, error.Description))
            .ToList();
        logger.LogError(
            "User registration failed for email: {0}.\nErrors: {1}", request.EmailAddress,
            string.Join(", ", errors.Select(e => $"{e.Code}: {e.Description}"))
        );
        return errors;
    }

    public async Task<ErrorOr<UserAuthResponse>> LoginAsync(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.EmailAddress);
        if (user is null)
        {
            logger.LogWarning("Email not found during login: {0}.", request.EmailAddress);
            return Errors.Auth.LoginFailed;
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (signInResult.Succeeded)
        {
            logger.LogInformation("User {0} logged in successfully.", user.Id);
            return new UserAuthResponse(Id: user.Id,
                Role: user.Role,
                AccessToken: tokenService.CreateUserJwt(user.Email!, user.Role, user.Id));
        }

        if (signInResult.IsLockedOut)
        {
            logger.LogInformation("User {0} is locked out. End date: {1}.\n\tRequest: {2}", user.Id, user.LockoutEnd, JsonSerializer.Serialize(request));
            return Errors.User.IsLockedOut;
        }

        if (signInResult.IsNotAllowed)
        {
            logger.LogInformation("User {0} is not allowed to access the system.\n\tRequest: {1}", user.Id, JsonSerializer.Serialize(request));
            return Errors.User.IsNotAllowed;
        }

        logger.LogError("Login failed for user {0}.\n\tRequest: {1}", user.Id, JsonSerializer.Serialize(request));
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
}