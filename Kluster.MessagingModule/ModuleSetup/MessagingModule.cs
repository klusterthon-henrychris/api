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

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Console.WriteLine($"Host: {rabbitMqSettings?.Host}. Username: {rabbitMqSettings?.Username} Password: {rabbitMqSettings?.Password}");
            
            if (env == Environments.Development)
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings?.Host, "/",
                        h =>
                        {
                            h.Username(rabbitMqSettings!.Username);
                            h.Password(rabbitMqSettings!.Password);
                        });

                    cfg.ConfigureEndpoints(context);
                });
            }

            else
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(rabbitMqSettings?.Host!), "/",
                        h =>
                        {
                            h.Username(rabbitMqSettings!.Username);
                            h.Password(rabbitMqSettings!.Password);
                        });

                    cfg.ConfigureEndpoints(context);
                });
            }
        });
    }
}