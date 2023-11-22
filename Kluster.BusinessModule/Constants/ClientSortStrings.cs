namespace Kluster.BusinessModule.Constants;

public static class ClientSortStrings
{
    public const string FirstNameAsc = nameof(FirstNameAsc);
    public const string FirstNameDesc = nameof(FirstNameDesc);
    // public const string LastNameAsc = nameof(LastNameAsc);
    // public const string LastNameDesc = nameof(LastNameDesc);
    // public const string BusinessNameAsc = nameof(BusinessNameAsc);
    // public const string BusinessNameDesc = nameof(BusinessNameDesc);
    // public const string CreatedDateAsc = nameof(CreatedDateAsc);
    // public const string CreatedDateDesc = nameof(CreatedDateDesc);

    // public static string[] AllOptions = [FirstNameAsc, FirstNameDesc];
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