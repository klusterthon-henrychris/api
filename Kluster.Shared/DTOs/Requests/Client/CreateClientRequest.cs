namespace Kluster.Shared.DTOs.Requests.Client;

public record CreateClientRequest(
    string FirstName,
    string LastName,
    string EmailAddress,
    string Address,
    string? BusinessName = null);