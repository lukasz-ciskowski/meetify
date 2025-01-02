using Microsoft.AspNetCore.SignalR;
namespace webapp.Hubs;

public class ChatHub:Hub
{
    public void JoinRoom(string roomId)
    {
        this.Groups.AddToGroupAsync(Context.ConnectionId, roomId);
    }
    
    public void LeaveRoom(string roomId)
    {
        this.Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
    }
}