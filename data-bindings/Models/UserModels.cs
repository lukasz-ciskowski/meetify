using System.Text.Json.Serialization;

namespace server.Models;

public class Auth0User
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Picture { get; set; }
}