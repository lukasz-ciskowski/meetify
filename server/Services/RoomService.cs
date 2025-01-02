using MongoDB.Bson;
using MongoDB.Driver;
using server.Contexts;
using server.Models;
using server.Repositories;

namespace server.Services;

public class RoomService: IRoomService
{
    private readonly MongoDbContext _db;
    private readonly IAuthRepository _auth;
    
    public RoomService(MongoDbContext db, IAuthRepository auth)
    {
        this._db = db;
        this._auth = auth;
    }

    public Task<List<Room>> GetRooms(string userId)
    {
        var rooms = _db.Rooms.Find(r => r.CreatorId == userId || r.Visibility == RoomVisibilities.Public || r.Collaborators.Contains(userId)).ToList();
        return Task.FromResult(rooms);
    }

    public Task<Room> AddRoom(Room room)
    {
        _db.Rooms.InsertOne(room);
        return Task.FromResult(room);
    }

    public async Task<RoomWithMessages> GetRoomWithMessages(string roomId, string userId)
    {
        var room = _db.Rooms.Find(r => r.Id == roomId).FirstOrDefault();
        if (room == null) throw new Exception("Room not found");
        var hasAccess = _hasAccessToRoom(userId, room);
        if (!hasAccess) throw new Exception("Access denied");
        
        // get first 20 messages
        var messages = _db.Messages.Find(m => m.RoomId == roomId).Limit(100).ToList();
        var uniqueSenderIds = messages.Select(m => m.SenderId).Distinct().ToList();
        var users = await _auth.GetUsers(uniqueSenderIds);

        return new RoomWithMessages
        {
            Room = room,
            Messages = messages.Select(m => new MessageWithUser
            {
                Message = m,
                User = users.FirstOrDefault(u => u?.UserId == m.SenderId)
            }).ToList(),
        };
    }
    
    private bool _hasAccessToRoom(string userId, Room room)
    {
        if (room.Visibility == RoomVisibilities.Public) return true;
        if (room.CreatorId == userId) return true;
        if (room.Collaborators.Contains(userId)) return true;
        return false;
    }
}