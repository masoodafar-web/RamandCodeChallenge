using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Data;
using UserManagement.Services.Contracts;
using UserManagement.Services.Implementions;

namespace UserManagement.Services;

public static class ServiceConfig
{
    public static IServiceCollection AddServiceLayer(this IServiceCollection service)
    {
        service.AddHttpContextAccessor();
        service.AddDataLayer();
        service.AddScoped<IUserService, UserService>();
        service.AddScoped<IAuthService, AuthService>();
        // service.AddScoped<IMessageQueueService, RabbitMqService>();
        service.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("amqps://sjzwkwap:IXnJwhWtrcWyGeMA-8dWk16hfhHXNG94@jackal.rmq.cloudamqp.com/sjzwkwap");
                cfg.ReceiveEndpoint("user-queue", e => { });
                cfg.ConfigureEndpoints(ctx);
            });
        });
        return service;
    }
}