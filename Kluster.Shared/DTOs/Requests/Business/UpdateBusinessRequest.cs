namespace Kluster.Shared.DTOs.Requests.Business;

public record UpdateBusinessRequest(
    string? Name,
    string? Address,
    string? RcNumber,
    string? Description,
    string? Industry);