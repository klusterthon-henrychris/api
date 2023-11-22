using Kluster.BusinessModule.Constants;
using Kluster.Shared.Constants;

namespace Kluster.BusinessModule.DTOs.Requests;

public record GetClientsRequest(
    int PageSize = SearchConstants.PageSize,
    int PageNumber = SearchConstants.PageNumber,
    string SortOption = ClientSortStrings.FirstNameAsc);