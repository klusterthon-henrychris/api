using Kluster.Shared.Constants;

namespace Kluster.Shared.DTOs.Responses.Requests;

public abstract class QueryStringParameters
{
    public int PageNumber { get; set; } = SearchConstants.PageNumber;
    private int _pageSize = SearchConstants.MinPageSize;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > SearchConstants.MaxPageSize ? SearchConstants.MaxPageSize : value;
    }
}