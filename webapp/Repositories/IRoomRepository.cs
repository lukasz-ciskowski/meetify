using server.Models;

namespace webapp.Repositories;

public interface IRoomRepository
{
    Task<List<Room>> GetRooms(string token);
    Task<Room> AddRoom(CreateRoomModel room, string token);
    Task<RoomWithMessages> GetRoom(string roomId, string token);
}