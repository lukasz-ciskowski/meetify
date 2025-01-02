using server.Models;

namespace server.Services;

public interface IMessageService
{
    public Task<MessageWithUser> AddMessage(Message message);
}