// See https://aka.ms/new-console-template for more information

using MassTransit;
using UserManagementConsoleApp;
var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.Host("amqps://sjzwkwap:IXnJwhWtrcWyGeMA-8dWk16hfhHXNG94@jackal.rmq.cloudamqp.com/sjzwkwap");

    cfg.ReceiveEndpoint("user-queue", e =>
    {
        e.Consumer<UserMessageConsumer>();
    });
});

await busControl.StartAsync();

try
{
    Console.WriteLine("Bus is running. Press any key to exit...");
    await Task.Run(() => Console.ReadKey());
}
finally
{
    await busControl.StopAsync();
}