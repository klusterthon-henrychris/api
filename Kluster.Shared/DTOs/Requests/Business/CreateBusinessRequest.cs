namespace Kluster.Shared.DTOs.Requests.Business;

public record CreateBusinessRequest(
    string BusinessName,
    string BusinessAddress,
    string CacNumber,
    string RcNumber,
    string Industry,
    string BusinessDescription = "");