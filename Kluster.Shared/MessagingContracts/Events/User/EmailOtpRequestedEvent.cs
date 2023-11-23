namespace Kluster.Shared.MessagingContracts.Events.User;

public record EmailOtpRequestedEvent(string FirstName, string LastName, string EmailAddress, string UserId);