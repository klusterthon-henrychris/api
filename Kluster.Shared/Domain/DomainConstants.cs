namespace Kluster.Shared.Domain;

public static class DomainConstants
{
    /// <summary>
    /// Use globally to enforce ID lengths
    /// </summary>
    public const int MaxIdLength = 450;

    public const int MinNameLength = 3;
    public const int MaxNameLength = 50;

    public const int MaxCacNumberLength = 15;
    public const int MaxRcNumberLength = 15;
    
    public const int MaxDescriptionLength = 200;
    
    public const int MinAddressLength = 3;
    public const int MaxAddressLength = 200;

    public const int MinEmailAddressLength = 5;
    public const int MaxEmailAddressLength = 50;

    public const int MaxJsonLength = 450;
    public const int MaxEnumLength = 20;
    
}