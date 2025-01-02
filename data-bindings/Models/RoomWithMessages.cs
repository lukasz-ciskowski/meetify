namespace server.Models;

public class RoomWithMessages
{
    public Room Room { get; set; }
    public List<MessageWithUser> Messages { get; set; }
}