using Azure;
using Azure.Security.KeyVault.Secrets;

namespace Kluster.Host;

public class KeyVaultPrefixManager(string prefix)
{
    private readonly string _prefix = $"{prefix}-";

    private string GetKey(string secretKey)
    {
        return secretKey[_prefix.Length..]
            .Replace("--", ConfigurationPath.KeyDelimiter);
    }

    public async Task<Dictionary<string, string>> GetAllSecretsWithPrefixAsync(SecretClient client)
    {
        var secretsWithPrefix = new Dictionary<string, string>();

        AsyncPageable<SecretProperties> secretProperties = client.GetPropertiesOfSecretsAsync();

        await foreach (var secretProperty in secretProperties)
        {
            if (!secretProperty.Name.StartsWith(_prefix))
            {
                continue;
            }

            var secret = await client.GetSecretAsync(secretProperty.Name);
            secretsWithPrefix.Add(GetKey(secret.Value.Name), secret.Value.Value);
        }

        return secretsWithPrefix;
    }
}