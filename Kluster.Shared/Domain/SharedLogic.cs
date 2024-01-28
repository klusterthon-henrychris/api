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
    
    public static string GetProdPostGresConnectionString()
    {
        var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        Console.WriteLine($"ConnUrl: {connUrl}");

        // Parse connection URL to connection string for Npgsql
        connUrl = connUrl.Replace("postgresql://", string.Empty);
        var pgUserPass = connUrl.Split("@")[0];
        var pgHostPortDb = connUrl.Split("@")[1];
        var pgHostPort = pgHostPortDb.Split("/")[0];
        var pgDb = pgHostPortDb.Split("/")[1];
        var pgUser = pgUserPass.Split(":")[0];
        var pgPass = pgUserPass.Split(":")[1];
        var pgHost = pgHostPort.Split(":")[0];
        var pgPort = pgHostPort.Split(":")[1];
        var updatedHost = pgHost;
        //.Replace("flycast", "internal");

        return $"Server={updatedHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};";
    }
}
