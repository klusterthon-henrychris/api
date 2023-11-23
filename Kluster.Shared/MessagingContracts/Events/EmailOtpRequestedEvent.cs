namespace Kluster.Shared.MessagingContracts.Events;

public record EmailOtpRequestedEvent(string FirstName, string LastName, string EmailAddress, string UserId);