using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Kluster.Host;

var builder = WebApplication.CreateBuilder(args);

using var scope = builder.Services.BuildServiceProvider().CreateScope();
var configuration = scope.ServiceProvider.GetService<IConfiguration>();

var keyVaultName = configuration["KeyVault:Vault"];
var kvUri = "https://" + keyVaultName + ".vault.azure.net";
var client = new SecretClient(new Uri(kvUri),
    new DefaultAzureCredential(new DefaultAzureCredentialOptions
        { ManagedIdentityClientId = configuration["KeyVault:ClientId"] }));
var sec = client.GetSecret("KlusterApi-MailSettings--Password");
Console.WriteLine($"Secret retrieved {sec}");
builder.Services.RegisterApplicationServices(builder.Environment);
builder.ConfigureSerilog();

var app = builder.Build();
app.ConfigureApplication();
app.Run();

// This is used for the integration tests.
namespace Kluster.Host
{
    public partial class Program;
}