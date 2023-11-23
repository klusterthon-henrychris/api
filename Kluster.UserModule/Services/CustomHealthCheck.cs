using Kluster.Shared.MessagingContracts;
using MassTransit;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Kluster.UserModule.Services;

public class CustomHealthCheck(IBus bus) : IHealthCheck
{
    // todo: perform full check for all important services.
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        try
        {
            await bus.Publish(new TestPayload("Checking RabbitMQ Status"), cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new HealthCheckResult(HealthStatus.Unhealthy);
        }

        return new HealthCheckResult(HealthStatus.Healthy);
    }
}