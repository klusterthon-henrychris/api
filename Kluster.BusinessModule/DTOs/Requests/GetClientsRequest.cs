using Kluster.BusinessModule.Constants;
using Kluster.Shared.Requests;

namespace Kluster.BusinessModule.DTOs.Requests;

public class GetClientsRequest : QueryStringParameters
{
    public string SortOption { get; set; } = ClientSortStrings.FirstNameAsc;
}