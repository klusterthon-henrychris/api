namespace Kluster.Shared.Configuration;

public class KeyVault
{
    public string AZURE_CLIENT_ID { get; set; }
    public string AZURE_CLIENT_SECRET { get; set; }
    public string AZURE_TENANT_ID { get; set; }
    public string AZURE_STORAGE_CONNECTION_STRING { get; set; }
    public string Vault { get; set; }
    public string BLOB_CONTAINER_NAME { get; set; }
}