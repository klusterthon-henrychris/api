namespace Kluster.Shared.MessagingContracts.Events;

public record PaymentNotificationReceived(string DataStatus, int DataAmount, string DataReference);