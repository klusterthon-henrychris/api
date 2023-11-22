using ErrorOr;
using Kluster.BusinessModule.DTOs.Requests;
using Kluster.BusinessModule.DTOs.Responses;
using Kluster.Shared.Requests;

namespace Kluster.BusinessModule.Services.Contracts;

public interface IClientService
{
    Task<ErrorOr<GetClientResponse>> GetClient(string id);
    Task<ErrorOr<CreateClientResponse>> CreateClientAsync(CreateClientRequest request);
    Task<ErrorOr<PagedList<GetClientResponse>>> GetAllClients(GetClientsRequest request);
    Task<ErrorOr<Updated>> UpdateClient(string clientId, UpdateClientRequest request);
}