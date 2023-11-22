namespace Kluster.BusinessModule.DTOs.Requests;

public record UpdateBusinessRequest(
    string? Name,
    string? Address,
    string? CacNumber,
    string? RcNumber,
    string? Description,
    string? Industry);