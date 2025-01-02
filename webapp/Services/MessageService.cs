using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using server.Models;
using webapp.Hubs;

namespace webapp.Services;

public class MessageService:IMessageService
{
    private readonly IHubContext<ChatHub> _chatContext;
    
    public MessageService(IHubContext<ChatHub> hubContext)
    {
        _chatContext = hubContext;
    }
    
    public async Task PublishMessageToClients(MessageWithUser messageContext)
    {
        var jsonMessage = JsonConvert.SerializeObject(messageContext);
        await _chatContext.Clients.Group(messageContext.Message.RoomId).SendAsync("ReceiveMessage", jsonMessage);
    }
}