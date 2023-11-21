namespace Kluster.BusinessModule.DTOs.Responses;

public record GetBusinessResponse(
    string Name,
    string Address,
    string CacNumber,
    string RcNumber,
    string Description,
    string Industry);