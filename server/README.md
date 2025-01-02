# Meetify - backend API

This project is an .NET Core Web Api based project that allows direct communication with the database.

## Usage

### Secrets

The only configuration you need to provide yourself is the **.NET User secrets**

The following configuration is required:
```json
{
  "MONGO_URI": "",
  "AUTH0_DOMAIN": "",
  "AUTH0_CLIENT": "",
  "AUTH0_CLIENT_SECRET": "",
  "AUTH0_AUDIENCE": "",
  "MQTT_USERNAME": "",
  "MQTT_PASSWORD": "",
  "MQTT_URL": "",
  "MQTT_PORT": 8883
}
```