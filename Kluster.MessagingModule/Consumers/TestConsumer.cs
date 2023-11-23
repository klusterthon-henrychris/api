using Kluster.Shared.MessagingContracts;
using MassTransit;

namespace Kluster.Messaging.Consumers;

public class TestConsumer(ILogger<TestConsumer> logger) : IConsumer<TestPayload>
{
    public Task Consume(ConsumeContext<TestPayload> context)
    {
        logger.LogInformation($"Health Check successful. Message: {context.Message.Message}");
        return Task.CompletedTask;
    }
}