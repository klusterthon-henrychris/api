using System.ComponentModel.DataAnnotations;
using ErrorOr;
using Kluster.Shared.API;
using Kluster.Shared.DTOs.Requests.Business;
using Microsoft.AspNetCore.Mvc;
using Kluster.Shared.Extensions;
using Kluster.Shared.SharedContracts.BusinessModule;

namespace Kluster.BusinessModule.Controllers;

public class BusinessController(IBusinessService businessService) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> CreateBusiness([Required, FromBody] CreateBusinessRequest request)
    {
        var createBusinessResult = await businessService.CreateBusinessAsync(request);

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
    public async Task<IActionResult> GetBusinessOfLoggedInUser()
    {
        var getBusinessResult = await businessService.GetBusinessOfLoggedInUser();

        // If successful, return the event data in an ApiResponse.
        // If an error occurs, return an error response using the ReturnErrorResponse method.
        return getBusinessResult.Match(
            _ => Ok(getBusinessResult.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateBusiness(UpdateBusinessRequest request)
    {
        var updateBusinessResult = await businessService.UpdateBusiness(request);
        return updateBusinessResult.Match(_ => NoContent(), ReturnErrorResponse);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBusiness()
    {
        var deleteBusinessResult = await businessService.DeleteBusiness();
        return deleteBusinessResult.Match(_ => NoContent(), ReturnErrorResponse);
    }
}