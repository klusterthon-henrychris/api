namespace Kluster.Shared.MessagingContracts.Events.Notification;

public record WelcomeUserEvent(string EmailAddress, string FirstName, string LastName);