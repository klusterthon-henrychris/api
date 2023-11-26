namespace Kluster.Shared.MessagingContracts.Events;

public record InvoicePaymentValidated(string InvoiceId, int AmountInKobo, string PaymentChannel);