using ErrorOr;
using Kluster.Shared.DTOs.Requests.Client;
using Kluster.Shared.DTOs.Responses.Client;
using Kluster.Shared.DTOs.Responses.Requests;

namespace Kluster.Shared.SharedContracts.BusinessModule;

public interface IClientService
{
    Task<ErrorOr<GetClientResponse>> GetClient(string id);
    Task<ErrorOr<CreateClientResponse>> CreateClientAsync(CreateClientRequest request);
    Task<ErrorOr<PagedList<GetClientResponse>>> GetAllClients(GetClientsRequest request);
    Task<ErrorOr<Updated>> UpdateClient(string clientId, UpdateClientRequest request);

    /// <summary>
    /// Gets the details needed for invoice creation.
    /// If the client does not exist, or does not belong to the business, an error is returned.
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    Task<ErrorOr<ClientAndBusinessResponse>> GetClientAndBusiness(string clientId);
    Task DeleteAllClientsRelatedToBusiness(string businessId);
}