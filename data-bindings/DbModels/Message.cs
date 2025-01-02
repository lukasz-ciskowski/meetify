using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace server.Models;

public class Message
{
    [BsonElement("_id")]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonElement("messageText")]
    public required string MessageText { get; set; }
    
    [BsonElement("senderId")]
    public required string SenderId { get; set; }
    
    [BsonElement("roomId")]
    public required string RoomId { get; set; }
    
    [BsonElement("timestamp")]
    public required DateTime Timestamp { get; set; } = DateTime.Now;
}