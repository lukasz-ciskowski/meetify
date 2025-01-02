using Microsoft.AspNetCore.SignalR;
namespace webapp.Hubs;

public class ChatHub:Hub
{
    public void JoinRoom(string roomId)
    {
        // Groups.Add(Context.ConnectionId, roomId);
        this.Groups.AddToGroupAsync(Context.ConnectionId, roomId);
    }
    
    public void LeaveRoom(string roomId)
    {
        // Groups.Remove(Context.ConnectionId, roomId);
        this.Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
    }
}