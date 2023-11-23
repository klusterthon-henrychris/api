namespace Kluster.Shared.DTOs.Requests.Business;

public record UpdateBusinessRequest(
    string? Name,
    string? Address,
    string? CacNumber,
    string? RcNumber,
    string? Description,
    string? Industry);