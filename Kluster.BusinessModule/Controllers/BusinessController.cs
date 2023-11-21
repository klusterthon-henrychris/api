using System.ComponentModel.DataAnnotations;
using Kluster.BusinessModule.DTOs.Requests;
using Kluster.BusinessModule.Services.Contracts;
using Kluster.Shared;
using Kluster.Shared.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Kluster.Shared.Extensions;

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
    
    // todo: businesses should only be able to view their client's businesses
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
}