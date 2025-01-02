using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using server.Models;
using webapp.Clients;
using webapp.Errors;
using webapp.Exceptions;

namespace webapp.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly IConfiguration _configuration;
    
    public RoomRepository(IConfiguration configuration)
    {
        this._configuration = configuration;
    }
    
    public async Task<List<Room>> GetRooms(string token)
    {
        using var client = new ServerClient(this._configuration).AddAuthHeader(token);

        var response = await client.PerformRequest(() => client.GetAsync("api/rooms"));

        return await response.Content.ReadFromJsonAsync<List<Room>>() ?? [];
    }

    public async Task<Room> AddRoom(CreateRoomModel room, string token)
    {
        using var client = new ServerClient(this._configuration).AddAuthHeader(token);

        var serializedRoom = JsonSerializer.Serialize(room);
        var body = new StringContent(serializedRoom, Encoding.UTF8, "application/json");
        var response = await client.PerformRequest(() => client.PostAsync("api/rooms", body));
        
        var result = await response.Content.ReadFromJsonAsync<Room>();
        if (result == null) throw new InternalException();
        
        return result;
    }

    public async Task<RoomWithMessages> GetRoom(string roomId, string token)
    {
        using var client = new ServerClient(this._configuration).AddAuthHeader(token);
        var response = await client.PerformRequest(() => client.GetAsync($"api/rooms/{roomId}"));
        var result = await response.Content.ReadFromJsonAsync<RoomWithMessages>();
        if (result == null) throw new InternalException();
        
        return result;
    }
}