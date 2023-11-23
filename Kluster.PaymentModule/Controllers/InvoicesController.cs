﻿using System.ComponentModel.DataAnnotations;
using Kluster.PaymentModule.Services.Contracts;
using Kluster.Shared.API;
using Microsoft.AspNetCore.Mvc;
using Kluster.Shared.Constants;
using Kluster.Shared.DTOs.Requests.Invoices;
using Kluster.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Kluster.PaymentModule.Controllers;

public class InvoicesController(IInvoiceService invoiceService) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> CreateInvoice([Required, FromBody] CreateInvoiceRequest request)
    {
        var createInvoiceResponse = await invoiceService.CreateInvoiceAsync(request);
        return createInvoiceResponse.Match(
            invoiceResponse => CreatedAtAction(nameof(GetInvoice), routeValues: new { id = invoiceResponse.Id },
                createInvoiceResponse.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetInvoice(string id)
    {
        var getInvoiceResult = await invoiceService.GetInvoice(id);

        // If successful, return the event data in an ApiResponse.
        // If an error occurs, return an error response using the ReturnErrorResponse method.
        return getInvoiceResult.Match(
            _ => Ok(getInvoiceResult.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = UserRoles.Admin)]
    [HttpPut("{InvoiceId}")]
    public async Task<IActionResult> UpdateInvoice(string invoiceId, [Required] UpdateInvoiceRequest request)
    {
        var updateUserResult = await invoiceService.UpdateInvoice(invoiceId, request);
        return updateUserResult.Match(_ => NoContent(), ReturnErrorResponse);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllInvoices([FromQuery] GetInvoicesRequest request)
    {
        var getInvoicesResult = await invoiceService.GetAllInvoices(request);

        // If successful, return the event data in an ApiResponse.
        // If an error occurs, return an error response using the ReturnErrorResponse method.
        return getInvoicesResult.Match(
            _ => Ok(getInvoicesResult.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }
}