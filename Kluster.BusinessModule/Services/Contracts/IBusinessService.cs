using Kluster.BusinessModule.DTOs.Requests;
using Kluster.BusinessModule.DTOs.Responses;
using ErrorOr;

namespace Kluster.BusinessModule.Services.Contracts;

public interface IBusinessService
{
    Task<ErrorOr<BusinessCreationResponse>> CreateBusinessAsync(CreateBusinessRequest request);
    Task<ErrorOr<GetBusinessResponse>> GetBusiness(string id);
    Task<ErrorOr<BusinessCreationResponse>> CreateClientBusinessAsync(CreateClientBusinessRequest request);
}