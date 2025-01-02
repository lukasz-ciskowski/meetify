using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using server.Models;

namespace server.Contexts;

public class MongoDbContext:DbContext
{
    private readonly IMongoDatabase _database;
    public MongoDbContext(String uri)
    {
        var client = new MongoClient(uri);
        this._database = client.GetDatabase("Cluster0");
    }
    

    public IMongoCollection<Room> Rooms => _database.GetCollection<Room>("Rooms");
    public IMongoCollection<Message> Messages => _database.GetCollection<Message>("Messages");
}