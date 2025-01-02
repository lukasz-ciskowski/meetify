using server.Models;

namespace webapp.Repositories;

public interface IMessageRepository
{
    Task<MessageWithUser> SendMessage(string roomId, SendMessageModel model, string token);
}