﻿using Kluster.Shared.Constants;
using Kluster.Shared.Domain;
using Kluster.Shared.DTOs.Requests.Invoices;
using Kluster.Shared.DTOs.Responses.Client;
using Kluster.Shared.DTOs.Responses.Invoices;
using Kluster.Shared.MessagingContracts.Commands.Payment;

namespace Kluster.PaymentModule;

public static class PaymentModuleMapper
{
    public static GetInvoiceResponse ToGetInvoiceResponse(Invoice invoice)
    {
        return new GetInvoiceResponse(invoice.InvoiceNo, invoice.Amount, invoice.DueDate, invoice.DateOfIssuance,
            invoice.Status, invoice.InvoiceItems);
    }

    public static Invoice ToInvoice(CreateInvoiceRequest request, ClientAndBusinessResponse clientAndBusinessResponse)
    {
        return new Invoice
        {
            Amount = request.Amount,
            DueDate = request.DueDate,
            Status = InvoiceStatus.Due.ToString(),
            BillingAddress = clientAndBusinessResponse.ClientBillingAddress,
            DateOfIssuance = DateTime.Now,
            InvoiceItems = request.InvoiceItems,
            ClientEmailAddress = clientAndBusinessResponse.ClientEmailAddress,
            ClientId = clientAndBusinessResponse.ClientId,
            BusinessId = clientAndBusinessResponse.BusinessId
        };
    }

    public static CreateInvoiceResponse ToCreateInvoiceResponse(Invoice invoice)
    {
        return new CreateInvoiceResponse(invoice.InvoiceNo);
    }

    public static DeletePaymentsForInvoice ToDeletePaymentForInvoice(Invoice invoice)
    {
        return new DeletePaymentsForInvoice(invoice.InvoiceNo);
    }
}