namespace Kluster.Shared.DTOs.Requests.Client;

public record UpdateClientRequest(
    string? FirstName,
    string? LastName,
    string? BusinessName,
    string? Address,
    string? EmailAddress);