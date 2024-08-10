using UserManagement.Models.DTOs;

namespace UserManagement.Services.Contracts;

public interface IMessageQueueService
{
    Task SendMessage<T>(T message);
}