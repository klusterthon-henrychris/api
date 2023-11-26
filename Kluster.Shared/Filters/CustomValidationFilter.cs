using Kluster.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Kluster.Shared.Filters
{
    public class CustomValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Check if the model state is invalid
            if (context.ModelState.IsValid) return;
            
            // Create a custom response
            var customErrorResponse = new ApiErrorResponse<object>(
                context.ModelState
                    .Where(x => x.Value is { Errors.Count: > 0 })
                    .SelectMany(kvp =>
                        kvp.Value?.Errors.Select(e => new { Code = kvp.Key, Description = e.ErrorMessage })!)
                    .ToList(),
                "One or more validation errors occured."
            );
                
            context.Result = new ObjectResult(customErrorResponse)
            {
                StatusCode = 400,
            };
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No action needed after the action is executed
        }
    }
}