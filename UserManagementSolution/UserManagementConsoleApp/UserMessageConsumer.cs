using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Models.DTOs;

namespace UserManagementConsoleApp;
public class UserMessageConsumer : IConsumer<NewUserCommandModel>
{

  

    public async Task Consume(ConsumeContext<NewUserCommandModel> context)
    {
        
        var message = context.Message;
        bool processed = false;//فرض اینکه پروسس روی این پیغام انجام نشده
        // بررسی اینکه آیا پیام پردازش شده است یا نه
        if (!processed)
        {
           //  var provider=new ServiceCollection().BuildServiceProvider().CreateScope();
           // var _sendEndpointProvider=context.GetSendEndpoint(new Uri("queue:new-queue"));
            // ارسال پیام به صف جدید با TTL ده ثانیه
            var endpoint = await context.GetSendEndpoint(new Uri("queue:new-queue"));

            await endpoint.Send(message, sendContext =>
            {
                sendContext.TimeToLive = TimeSpan.FromSeconds(10); // تنظیم TTL برای پیام
            });

            Console.WriteLine($"Message for user {message.Username} has been moved to the new queue.");
        }
        else
        {
            Console.WriteLine($"Message for user {message.Username} has been processed.");
        }
    }
}
