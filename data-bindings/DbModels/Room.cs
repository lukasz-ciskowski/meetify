using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace server.Models;

public class Room
{
    [BsonElement("_id")]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonElement("title")]
    public required string Title { get; set; }
    
    [BsonElement("description")]
    public required string Description { get; set; }
    
    [BsonElement("creatorId")]
    public required string CreatorId { get; set; }
    
    [BsonElement("visibility")]
    public required RoomVisibilities Visibility { get; set; }

    [BsonElement("collaborators")] public List<string> Collaborators { get; set; } = new List<string>();
}