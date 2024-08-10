using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using UserManagement.Models.DTOs;
using UserManagement.Services.Contracts;

namespace UserManagement.Services.Implementions;

public class RabbitMqService:IMessageQueueService
{
    private readonly IConfiguration _configuration;

    public RabbitMqService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendMessage<T>(T message)
    {
        var factory = new ConnectionFactory() { HostName = _configuration["RabbitInfo:Hostname"] };
        using (var connection =await factory.CreateConnectionAsync())
        using (var channel =await connection.CreateChannelAsync())
        {
            channel.QueueDeclareAsync(queue: _configuration["RabbitInfo:QueueName"],
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            channel.BasicPublishAsync(exchange: "",
                routingKey: _configuration["RabbitInfo:QueueName"],
                body: body);
        }
    }
}