namespace Kluster.Shared.Domain;

public static class SharedLogic
{
    public static string GenerateReference(string prefix)
    {
        var startDate = DomainConstants.ReferenceStartDate;
        var now = DateTime.UtcNow;
        var offset = (int)(now - startDate).TotalSeconds;
        return string.Join("-", prefix, now.ToString("yyyyMMddHHmmss"), offset);
    }
}