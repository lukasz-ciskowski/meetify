using System.Text;
using System.Text.Json;
using server.Models;
using webapp.Clients;
using webapp.Errors;

namespace webapp.Repositories;

public class MessageRepository:IMessageRepository
{
    private readonly IConfiguration _configuration;
    
    public MessageRepository(IConfiguration configuration)
    {
        this._configuration = configuration;
    }
    
    public async Task<MessageWithUser> SendMessage(string roomId, SendMessageModel model, string token)
    {
        using var client = new ServerClient(this._configuration).AddAuthHeader(token);

        var serializedRoom = JsonSerializer.Serialize(model);
        var body = new StringContent(serializedRoom, Encoding.UTF8, "application/json");
        var response = await client.PerformRequest(() => client.PostAsync($"api/rooms/{roomId}/messages", body));
        
        var result = await response.Content.ReadFromJsonAsync<MessageWithUser>();
        if (result == null) throw new InternalException();
        
        return result;
    }
}