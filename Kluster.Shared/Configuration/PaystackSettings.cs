namespace Kluster.Shared.Configuration;

public class PaystackSettings
{
    public string PublicKey { get; set; }
    public string SecretKey { get; set; }
    public string[] AllowedIps { get; set; }
}