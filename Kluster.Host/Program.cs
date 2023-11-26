using Kluster.Host;
using Kluster.Shared.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);
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