﻿using Kluster.Shared.Constants;
using Kluster.Shared.Domain;
using Kluster.Shared.DTOs.Requests.Invoices;
using Kluster.Shared.DTOs.Responses.Client;
using Kluster.Shared.DTOs.Responses.Invoices;

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
            BillingAddress = clientAndBusinessResponse.ClientAddress,
            DateOfIssuance = DateTime.Now,
            InvoiceItems = request.InvoiceItems,
            ClientId = clientAndBusinessResponse.ClientId,
            BusinessId = clientAndBusinessResponse.BusinessId
        };
    }

    public static CreateInvoiceResponse ToCreateInvoiceResponse(Invoice invoice)
    {
        return new CreateInvoiceResponse(invoice.InvoiceNo);
    }
}