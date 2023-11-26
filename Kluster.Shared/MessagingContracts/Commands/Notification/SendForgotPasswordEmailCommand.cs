namespace Kluster.Shared.MessagingContracts.Commands.Notification;

public record SendForgotPasswordEmailCommand(string EmailAddress, string Token, string FirstName, string LastName);