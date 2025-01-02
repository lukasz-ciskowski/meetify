using System.Text;
using HiveMQtt.Client;
using HiveMQtt.MQTT5.Types;
using Newtonsoft.Json;
using server.Models;
using webapp.Services;

namespace webapp.Clients;

public class MqttBroker: IMqttBroker
{
    private readonly IMessageService _messageService;
    
    public MqttBroker(HiveMQClient client, IMessageService messageService)
    {
        Console.WriteLine("MqttBroker created");
        this._messageService = messageService;
        
        client.OnMessageReceived += (sender, args) =>
        {
            var topic = args.PublishMessage.Topic ?? "";
            if (topic.StartsWith("rooms/")) OnHandleRoomMessage(args.PublishMessage);
        };
        client.SubscribeAsync("rooms/#").ConfigureAwait(false);
    }
    
    public void OnHandleRoomMessage(MQTT5PublishMessage message)
    {
        var payload = JsonConvert.DeserializeObject<MessageWithUser>(message.PayloadAsString);
        _messageService.PublishMessageToClients(payload);
    }
}