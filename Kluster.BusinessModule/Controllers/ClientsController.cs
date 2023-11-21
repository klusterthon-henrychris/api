using System.ComponentModel.DataAnnotations;
using Kluster.BusinessModule.DTOs.Requests;
using Kluster.BusinessModule.Services.Contracts;
using Kluster.Shared.API;
using Microsoft.AspNetCore.Mvc;
using Kluster.Shared.Extensions;

namespace Kluster.BusinessModule.Controllers;

public class ClientsController(IClientService clientService) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> CreateClient([Required, FromBody] CreateClientRequest request)
    {
        var createClientResponse = await clientService.CreateClientAsync(request);
        return createClientResponse.Match(
            clientResponse => CreatedAtAction(nameof(GetClient), routeValues: new { id = clientResponse.Id },
                createClientResponse.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClient(string id)
    {
        var getClientResult = await clientService.GetClient(id);

        // If successful, return the event data in an ApiResponse.
        // If an error occurs, return an error response using the ReturnErrorResponse method.
        return getClientResult.Match(
            _ => Ok(getClientResult.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }
}