namespace Kluster.BusinessModule.DTOs.Requests;

public record UpdateClientRequest(
    string? FirstName,
    string? LastName,
    string? BusinessName,
    string? Address,
    string? EmailAddress);