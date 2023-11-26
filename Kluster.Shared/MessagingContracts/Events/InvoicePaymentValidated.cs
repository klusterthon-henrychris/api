namespace Kluster.Shared.MessagingContracts.Events;

public record InvoicePaymentValidated(string InvoiceId, int Amount, string PaymentChannel);