using ErrorOr;
using Kluster.BusinessModule.DTOs.Requests;
using Kluster.BusinessModule.DTOs.Responses;

namespace Kluster.BusinessModule.Services.Contracts;

public interface IClientService
{
    Task<ErrorOr<GetClientResponse>> GetClient(string id);
    Task<ErrorOr<CreateClientResponse>> CreateClientAsync(CreateClientRequest request);
}