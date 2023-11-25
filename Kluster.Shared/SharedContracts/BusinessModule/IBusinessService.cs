﻿using ErrorOr;
using Kluster.Shared.DTOs.Requests.Business;
using Kluster.Shared.DTOs.Responses.Business;

namespace Kluster.Shared.SharedContracts.BusinessModule;

public interface IBusinessService
{
    Task<ErrorOr<BusinessCreationResponse>> CreateBusinessAsync(CreateBusinessRequest request);
    Task<ErrorOr<GetBusinessResponse>> GetBusinessById(string id);

    /// <summary>
    /// Calls ICurrentUser to get the userId behind the current request.
    /// Uses that to fetch their businessId. Returns error if not found.
    /// </summary>
    /// <returns></returns>
    Task<ErrorOr<string>> GetBusinessIdOnly();

    Task<ErrorOr<GetBusinessResponse>> GetBusinessOfLoggedInUser();
    Task<ErrorOr<Updated>> UpdateBusiness(UpdateBusinessRequest request);
    Task<ErrorOr<Deleted>> DeleteBusiness();
}