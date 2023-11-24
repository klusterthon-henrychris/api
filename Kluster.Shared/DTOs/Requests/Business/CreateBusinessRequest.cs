namespace Kluster.Shared.DTOs.Requests.Business;

public record CreateBusinessRequest(
    string BusinessName,
    string BusinessAddress,
    string RcNumber,
    string Industry,
    string BusinessDescription = "");