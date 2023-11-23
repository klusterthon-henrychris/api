namespace Kluster.Shared.DTOs.Responses.Client;

public record GetClientResponse(
    string FirstName,
    string LastName,
    string EmailAddress,
    string BusinessName,
    string Address);