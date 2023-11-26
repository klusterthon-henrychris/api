using Kluster.Host;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterApplicationServices(builder.Environment);

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console(new JsonFormatter())
    .WriteTo.Seq("http://localhost:5341")
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
);

var app = builder.Build();
app.ConfigureApplication();
app.Run();

// This is used for the integration tests.
namespace Kluster.Host
{
    public partial class Program;
}