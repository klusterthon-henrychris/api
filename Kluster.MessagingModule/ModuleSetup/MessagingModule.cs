using System.Reflection;
using Kluster.Shared.Configuration;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Kluster.Messaging.ModuleSetup;

public static class MessagingModule
{
    public static void AddMessagingModule(this IServiceCollection services)
    {
        var rabbitMqSettings = services.BuildServiceProvider().GetService<IOptionsSnapshot<RabbitMqSettings>>()?.Value;

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            
            var assembly = Assembly.GetAssembly(typeof(MessagingModule));
            x.AddConsumers(assembly);
            x.AddSagaStateMachines(assembly);
            x.AddSagas(assembly);
            x.AddActivities(assembly);
            
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqSettings?.Host ?? throw new InvalidOperationException("RabbitMQ Host not Set."), "/",
                    h =>
                    {
                        // todo: get from secrets
                        h.Username(rabbitMqSettings.Username);
                        h.Password(rabbitMqSettings.Password);
                    });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}