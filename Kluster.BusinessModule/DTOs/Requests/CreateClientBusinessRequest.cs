namespace Kluster.BusinessModule.DTOs.Requests;

public record CreateClientBusinessRequest(
    string BusinessName,
    string BusinessAddress,
    string Industry,
    string ClientId,
    string BusinessDescription = "");