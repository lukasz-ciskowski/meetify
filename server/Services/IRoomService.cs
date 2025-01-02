using server.Models;

namespace server.Services;

public interface IRoomService
{
    public Task<List<Room>> GetRooms(string userId);
    public Task<Room> AddRoom(Room room);
    public Task<RoomWithMessages> GetRoomWithMessages(string roomId, string userId);
}