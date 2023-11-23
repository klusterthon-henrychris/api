namespace Kluster.Shared.Constants;

public static class ClientSortStrings
{
    public const string FirstNameAsc = nameof(FirstNameAsc);
}

public enum ClientSortOptions
{
    FirstNameAsc,
    FirstNameDesc,
    LastNameAsc,
    LastNameDesc,
    BusinessNameAsc,
    BusinessNameDesc,
    CreatedDateAsc,
    CreatedDateDesc
}