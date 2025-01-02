using HiveMQtt.MQTT5.Types;

namespace webapp.Clients;

public interface IMqttBroker
{ 
    public void OnHandleRoomMessage(MQTT5PublishMessage message);
}