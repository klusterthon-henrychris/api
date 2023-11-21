namespace Kluster.BusinessModule.DTOs.Responses;

public record GetClientResponse(
    string FirstName,
    string LastName,
    string EmailAddress,
    string BusinessName,
    string Address);