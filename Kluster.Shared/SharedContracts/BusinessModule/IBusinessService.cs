using ErrorOr;
using Kluster.Shared.DTOs.Requests.Business;
using Kluster.Shared.DTOs.Responses.Business;

namespace Kluster.Shared.SharedContracts.BusinessModule;

public interface IBusinessService
{
    Task<ErrorOr<BusinessCreationResponse>> CreateBusinessAsync(CreateBusinessRequest request);
    Task<ErrorOr<GetBusinessResponse>> GetBusinessById(string id);
    Task<ErrorOr<string>> GetBusinessId();
    Task<ErrorOr<GetBusinessResponse>> GetBusinessOfLoggedInUser();
    Task<ErrorOr<Updated>> UpdateBusiness(UpdateBusinessRequest request);
    Task<ErrorOr<Deleted>> DeleteBusiness();
}