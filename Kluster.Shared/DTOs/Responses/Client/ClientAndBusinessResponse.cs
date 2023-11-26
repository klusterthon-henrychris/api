namespace Kluster.Shared.DTOs.Responses.Client;

public record ClientAndBusinessResponse(
    string ClientId,
    string BusinessId,
    string ClientBillingAddress,
    string ClientEmailAddress,
    string FirstName,
    string LastName,
    string BusinessName);