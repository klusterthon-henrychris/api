using ErrorOr;
using Kluster.Shared.DTOs.Requests.Client;
using Kluster.Shared.DTOs.Responses.Client;
using Kluster.Shared.DTOs.Responses.Requests;

namespace Kluster.Shared.SharedContracts;

public interface IClientService
{
    Task<ErrorOr<GetClientResponse>> GetClient(string id);
    Task<ErrorOr<CreateClientResponse>> CreateClientAsync(CreateClientRequest request);
    Task<ErrorOr<PagedList<GetClientResponse>>> GetAllClients(GetClientsRequest request);
    Task<ErrorOr<Updated>> UpdateClient(string clientId, UpdateClientRequest request);
}