namespace Kluster.BusinessModule.DTOs.Requests;

public record CreateBusinessRequest(
    string BusinessName,
    string BusinessAddress,
    string CacNumber,
    string RcNumber,
    string Industry,
    string BusinessDescription = "");