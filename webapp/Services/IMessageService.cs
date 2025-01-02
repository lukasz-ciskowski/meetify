using server.Models;

namespace webapp.Services;

public interface IMessageService
{
    Task PublishMessageToClients(MessageWithUser messageContext);
}