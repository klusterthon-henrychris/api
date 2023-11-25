using System.ComponentModel.DataAnnotations;
using Kluster.Shared.API;
using Kluster.Shared.DTOs.Requests.Business;
using Kluster.Shared.DTOs.Requests.Wallet;
using Microsoft.AspNetCore.Mvc;
using Kluster.Shared.Extensions;
using Kluster.Shared.SharedContracts.BusinessModule;

namespace Kluster.BusinessModule.Controllers;

public class BusinessController(IBusinessService businessService) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> CreateBusinessForCurrentUser([Required, FromBody] CreateBusinessRequest request)
    {
        var createBusinessResult = await businessService.CreateBusinessForCurrentUser(request);

        return createBusinessResult.Match(
            businessResponse => CreatedAtAction(nameof(GetBusinessById), routeValues: new { id = businessResponse.Id },
                createBusinessResult.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetBusinessById(string id)
    {
        var getBusinessResult = await businessService.GetBusinessById(id);

        // If successful, return the event data in an ApiResponse.
        // If an error occurs, return an error response using the ReturnErrorResponse method.
        return getBusinessResult.Match(
            _ => Ok(getBusinessResult.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }

    [HttpGet]
    public async Task<IActionResult> GetBusinessForCurrentUser()
    {
        var getBusinessResult = await businessService.GetBusinessForCurrentUser();

        // If successful, return the event data in an ApiResponse.
        // If an error occurs, return an error response using the ReturnErrorResponse method.
        return getBusinessResult.Match(
            _ => Ok(getBusinessResult.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateBusinessForCurrentUser(UpdateBusinessRequest request)
    {
        var updateBusinessResult = await businessService.UpdateBusinessForCurrentUser(request);
        return updateBusinessResult.Match(_ => NoContent(), ReturnErrorResponse);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBusinessForCurrentUser()
    {
        var deleteBusinessResult = await businessService.DeleteBusinessForCurrentUser();
        return deleteBusinessResult.Match(_ => NoContent(), ReturnErrorResponse);
    }

    [HttpGet("wallet/balance")]
    public async Task<IActionResult> GetBusinessWalletBalance()
    {
        var getBalanceResult = await businessService.GetBusinessWalletBalance();

        // If successful, return the event data in an ApiResponse.
        // If an error occurs, return an error response using the ReturnErrorResponse method.
        return getBalanceResult.Match(
            _ => Ok(getBalanceResult.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }
}