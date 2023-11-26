using System.Web;
using ErrorOr;
using Kluster.Shared.Constants;
using Kluster.Shared.Domain;
using Kluster.Shared.DTOs.Requests.User;
using Kluster.Shared.DTOs.Responses.User;
using Kluster.Shared.Exceptions;
using Kluster.Shared.Extensions;
using Kluster.Shared.MessagingContracts.Commands.Notification;
using Kluster.Shared.MessagingContracts.Events.Notification;
using Kluster.Shared.MessagingContracts.Events.User;
using Kluster.Shared.ServiceErrors;
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
            logger.LogInformation($"User not found for ID: {userId}");
            return Errors.User.NotFound;
        }

        logger.LogInformation($"Retrieved logged-in user: {user.Id}");
        return UserModuleMapper.ToUserResponse(user);
    }

    public async Task<ErrorOr<Updated>> UpdateUser(UpdateUserRequest request)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        logger.LogInformation($"Received request to update user from {userId}.\nRequest: {request}");
        var validateResult = await new UpdateUserRequestValidator().ValidateAsync(request);
        if (!validateResult.IsValid)
        {
            logger.LogInformation($"UpdateUser request validation failed");
            return validateResult.ToErrorList();
        }

        var user = await context.Users.FindAsync(userId);
        if (user is null)
        {
            logger.LogInformation($"User not found for ID: {userId}");
            return Errors.User.NotFound;
        }

        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;
        user.Address = request.Address ?? user.Address;

        context.Update(user);
        await context.SaveChangesAsync();
        logger.LogInformation($"User updated: {user.Id}");
        return Result.Updated;
    }

    public async Task<string> GenerateOtpForEmail(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            logger.LogInformation($"User not found for ID: {userId}");
            throw new UserNotFoundException();
        }

        var otp = await userManager.GenerateEmailConfirmationTokenAsync(user);
        logger.LogInformation($"Generated OTP for user: {user.Id}");
        return HttpUtility.UrlEncode(otp);
    }

    public async Task<ErrorOr<Success>> ConfirmEmailWithToken(string emailAddress, string otp)
    {
        var user = await userManager.FindByEmailAsync(emailAddress);
        if (user is null)
        {
            logger.LogInformation($"User not found for email: {emailAddress}");
            return Errors.User.NotFound;
        }

        var result = await userManager.ConfirmEmailAsync(user, otp);
        if (result.Succeeded)
        {
            logger.LogInformation($"Email confirmed for user: {user.Id}");
            await bus.Publish(new WelcomeUserEvent(user.Email!, user.FirstName, user.LastName));
            return Result.Success;
        }

        var errors = result.Errors
            .Select(error => Error.Validation("User." + error.Code, error.Description))
            .ToList();
        logger.LogInformation($"Email confirmation failed for user: {user.Id}");
        return errors;
    }

    /// <summary>
    /// This resends the email or sms to the user trying to verify their email or phoneNumber
    /// </summary>
    /// <param name="id"></param>
    /// <param name="verificationRoute">Can be either Email or Phone.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException">Throws an exception when the choice isn't email.</exception>
    public async Task<ErrorOr<Success>> ResendVerificationMessage(string id, string verificationRoute)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
        {
            // for security reasons, don't say User not found.
            logger.LogInformation($"User not found for ID: {id}");
            return Errors.User.ReconfirmEmail;
        }

        // todo: if there's time, use the email instead of the id above ^. email more secure than displaying ID.
        if (string.Equals(verificationRoute, OtpRoute.Email.ToString(), StringComparison.CurrentCultureIgnoreCase))
        {
            logger.LogInformation($"Resending verification message to user: {user.Id} (Email: {user.Email})");
            await bus.Publish(new EmailOtpRequestedEvent(user.FirstName, user.LastName, user.Email!, user.Id));
            return Result.Success;
        }

        logger.LogInformation($"SMS Validation not available for user: {user.Id}");
        throw new NotImplementedException("SMS Validation not available.");
    }

    public async Task<ErrorOr<Success>> SendForgotPasswordMail(ForgotPasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.EmailAddress);
        if (user is null)
        {
            // for security reasons, don't say User not found.
            // say email will be sent.
            logger.LogInformation($"User not found for email: {request.EmailAddress}");
            return Result.Success;
        }

        logger.LogInformation($"Sending forgot password email to user: {user.Id} (Email: {user.Email})");
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        await bus.Publish(new SendForgotPasswordEmailCommand(user.Email!, token, user.FirstName, user.LastName));
        return Result.Success;
    }

    public async Task<ErrorOr<Success>> ResetPassword(ResetPasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.EmailAddress);
        if (user is null)
        {
            // ask to reconfirm email
            logger.LogInformation($"User not found for email: {request.EmailAddress}");
            return Errors.User.ReconfirmEmail;
        }

        logger.LogInformation($"Resetting password for user: {user.Id} (Email: {user.Email})");
        var resetPassResult = await userManager.ResetPasswordAsync(user, request.Token, request.Password);
        if (!resetPassResult.Succeeded)
        {
            logger.LogError($"ResetPassword failed for user {user.Id}.");
            var errors = resetPassResult.Errors
                .Select(error => Error.Validation("User." + error.Code, error.Description))
                .ToList();
            logger.LogInformation($"Password reset failed for user: {user.Id}");
            return errors;
        }

        logger.LogInformation($"Password reset successful for user: {user.Id}");
        return Result.Success;
    }
}