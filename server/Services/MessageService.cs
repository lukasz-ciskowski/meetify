using HiveMQtt.Client;
using Newtonsoft.Json;
using server.Contexts;
using server.Models;
using server.Repositories;

namespace server.Services;

public class MessageService: IMessageService
{
    private readonly MongoDbContext _db;
    private readonly HiveMQClient _mqttClient;
    private readonly IAuthRepository _authRepository;
    
    public MessageService(MongoDbContext db, HiveMQClient mqttClient, IAuthRepository authRepository)
    {
        this._db = db;
        this._mqttClient = mqttClient;
        this._authRepository = authRepository;
    }

    public async Task<MessageWithUser> AddMessage(Message message)
    {  
         await _db.Messages.InsertOneAsync(message);
         
         var user = await _authRepository.GetUser(message.SenderId);
         
         var messageWithUser = new MessageWithUser
         {
             Message = message,
             User = user
         };
         var jsonMessage = JsonConvert.SerializeObject(messageWithUser);
         _ = _mqttClient.PublishAsync($"rooms/{message.RoomId}", jsonMessage);
         return messageWithUser;
    }
}