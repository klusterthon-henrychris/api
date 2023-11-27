using ErrorOr;
using Kluster.Shared.DTOs.Requests.Business;
using Kluster.Shared.DTOs.Requests.Wallet;
using Kluster.Shared.DTOs.Responses.Business;

namespace Kluster.Shared.SharedContracts.BusinessModule;

public interface IBusinessService
{
    Task<ErrorOr<BusinessCreationResponse>> CreateBusinessForCurrentUser(CreateBusinessRequest request);
    Task<ErrorOr<GetBusinessResponse>> GetBusinessById(string id);

    /// <summary>
    /// Calls ICurrentUser to get the userId behind the current request.
    /// Uses that to fetch their businessId. Returns error if not found.
    /// </summary>
    /// <returns></returns>
    Task<ErrorOr<string>> GetBusinessIdOnlyForCurrentUser();

    Task<ErrorOr<GetBusinessResponse>> GetBusinessForCurrentUser();
    Task<ErrorOr<Updated>> UpdateBusinessForCurrentUser(UpdateBusinessRequest request);
    Task<ErrorOr<Deleted>> DeleteBusinessForCurrentUser();
    Task<ErrorOr<GetWalletBalanceResponse>> GetBusinessWalletBalance();

    Task<string?> GetBusinessName(string businessId);
}