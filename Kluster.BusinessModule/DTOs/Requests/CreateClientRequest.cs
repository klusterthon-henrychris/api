namespace Kluster.BusinessModule.DTOs.Requests;

public record CreateClientRequest(
    string FirstName,
    string LastName,
    string EmailAddress,
    string Address,
    string? BusinessName = null);