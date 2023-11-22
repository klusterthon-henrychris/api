using ErrorOr;
using Kluster.Shared.Filters;
using Kluster.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kluster.Shared.API;

[Authorize]
[ApiController]
[TypeFilter(typeof(CustomValidationFilter))]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Returns an IActionResult object based on the list of errors passed as parameter.
    /// </summary>
    /// <param name="errors">List of errors to be handled.</param>
    /// <returns>An IActionResult object based on the type of errors.</returns>
    protected static IActionResult ReturnErrorResponse(List<Error> errors)
    {
        var errorMessage = "One or more errors occurred.";
        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            errorMessage = "One or more validation errors occured.";
        }

        if (errors.Any(e => e.Type == ErrorType.Unexpected))
        {
            errorMessage = "Something went wrong.";
        }

        var firstError = errors[0];
        var statusCode = firstError.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var finalErrors = errors.Select(x => new { x.Code, x.Description }).ToList();
        var problemDetails = new ApiErrorResponse<object>(finalErrors, errorMessage);
        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode
        };
    }
}