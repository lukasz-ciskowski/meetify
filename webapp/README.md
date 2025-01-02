# Meetify - web interface

This project is an ASP.NET MVC based project used as an browser website for the Meetify application.

## Usage

### Styles

This project uses Tailwind as a css provider. Please install tailwind locally on your machine and use the command `npm run tailwind:watch` to generate the tailwind styles. You can follow up with [this article](https://medium.com/@omerconsept999/how-can-you-use-tailwindcss-with-net-core-mvc-445694739a6e)

### Secrets

The only configuration you need to provide yourself is the **.NET User secrets**

The following configuration is required:
```json 
{
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