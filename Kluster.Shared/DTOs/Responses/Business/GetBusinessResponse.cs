namespace Kluster.Shared.DTOs.Responses.Business;

public record GetBusinessResponse(
    string Name,
    string Address,
    string CacNumber,
    string RcNumber,
    string Description,
    string Industry);