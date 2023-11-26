namespace Kluster.Shared.DTOs.Responses.Business;

public record GetBusinessResponse(
    string Id,
    string Name,
    string Address,
    string RcNumber,
    string Description,
    string Industry);