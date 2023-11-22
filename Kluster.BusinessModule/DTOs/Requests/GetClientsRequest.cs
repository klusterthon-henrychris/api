using Kluster.BusinessModule.Constants;
using Kluster.Shared.Requests;

namespace Kluster.BusinessModule.DTOs.Requests;

public class GetClientsRequest : QueryStringParameters
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? BusinessName { get; set; }
    public string? EmailAddress { get; set; }
    public string? Address { get; set; }
    public string SortOption { get; set; } = ClientSortStrings.FirstNameAsc;
}