using System.Web;
using ErrorOr;
using Kluster.Shared.Constants;
using Kluster.Shared.Domain;
using Kluster.Shared.DTOs.Requests.User;
using Kluster.Shared.DTOs.Responses.User;
using Kluster.Shared.Exceptions;
using Kluster.Shared.Extensions;
using Kluster.Shared.MessagingContracts.Events.Notification;
using Kluster.Shared.MessagingContracts.Events.User;
using Kluster.Shared.SharedContracts.UserModule;
using Kluster.UserModule.Data;
using Kluster.UserModule.ServiceErrors;
using Kluster.UserModule.Validators;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace Kluster.UserModule.Services;

public class UserService(
    ICurrentUser currentUser,
    UserManager<ApplicationUser> userManager,
    IBus bus,
    ILogger<UserService> logger,
    UserModuleDbContext context) : IUserService
{
    public async Task<ErrorOr<UserResponse>> GetLoggedInUser()
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        var user = await context.Users.FindAsync(userId);
        if (user is null)
        {
            return Errors.User.NotFound;
        }

        return UserModuleMapper.ToUserResponse(user);
    }

    public async Task<ErrorOr<Updated>> UpdateUser(UpdateUserRequest request)
    {
        var validateResult = await new UpdateUserRequestValidator().ValidateAsync(request);
        if (!validateResult.IsValid)
        {
            return validateResult.ToErrorList();
        }

        var userId = currentUser.UserId ?? throw new UserNotSetException();
        var user = await context.Users.FindAsync(userId);
        if (user is null)
        {
            return Errors.User.NotFound;
        }

        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;
        user.Address = request.Address ?? user.Address;

        context.Update(user);
        await context.SaveChangesAsync();
        return Result.Updated;
    }

    public async Task<string> GenerateOtpForEmail(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        var otp = await userManager.GenerateEmailConfirmationTokenAsync(user);
        logger.LogInformation($"Generated OTP for {user.Id}.");
        return otp;
    }

    public async Task<ErrorOr<Success>> ConfirmEmailWithToken(string userId, string otp)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Errors.User.NotFound;
        }

        var result = await userManager.ConfirmEmailAsync(user, otp);
        if (result.Succeeded)
        {
            await bus.Publish(new WelcomeUserEvent(user.Email!, user.FirstName, user.LastName));
            return Result.Success;
        }

        var errors = result.Errors
            .Select(error => Error.Validation("User." + error.Code, error.Description))
            .ToList();
        return errors;
    }

    /// <summary>
    /// This resends the email or sms to the user trying to verify their email or phoneNumber
    /// </summary>
    /// <param name="id"></param>
    /// <param name="verificationRoute">Can be either Email or Phone.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<ErrorOr<Success>> ResendVerificationMessage(string id, string verificationRoute)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
        {
            return Errors.User.NotFound;
        }

        if (string.Equals(verificationRoute, OtpRoute.Email.ToString(), StringComparison.CurrentCultureIgnoreCase))
        {
            await bus.Publish(new EmailOtpRequestedEvent(user.FirstName, user.LastName, user.Email!, user.Id));
            return Result.Success;
        }

        throw new NotImplementedException("SMS Validation not available.");
    }
}