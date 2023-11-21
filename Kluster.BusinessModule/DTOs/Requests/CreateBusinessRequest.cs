namespace Kluster.BusinessModule.DTOs.Requests;

// businessAddress should be empty, for now.
// todo: when communication is set, address should fall back to user address field. ask frontend to pass userAddress
public record CreateBusinessRequest(
    string BusinessName,
    string BusinessAddress,
    string CacNumber,
    string RcNumber,
    string Industry,
    string BusinessDescription = "");