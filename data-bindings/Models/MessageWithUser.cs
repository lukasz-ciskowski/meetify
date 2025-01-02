namespace server.Models;

public class MessageWithUser
{
    public Message Message { get; set; }
    public Auth0User User { get; set; }
}