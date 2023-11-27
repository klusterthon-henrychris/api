using Kluster.Host;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterApplicationServices();
builder.ConfigureSerilog();

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
Console.WriteLine($"PORT: {port}.");
builder.WebHost.UseUrls($"http://*:{port}");
var app = builder.Build();
app.ConfigureApplication();
app.Run();

// This is used for the integration tests.
namespace Kluster.Host
{
    public partial class Program;
}