using System.Security.Claims;
using HiveMQtt.Client;
using HiveMQtt.Client.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using server.Contexts;
using server.Repositories;
using server.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

builder.Services.AddSingleton(sp => new MongoDbContext(config["MONGO_URI"] ?? ""));

var options = new HiveMQClientOptions
{
    Host = config["MQTT_URL"] ?? "",
    Port = int.Parse(config["MQTT_PORT"] ?? string.Empty),
    UserName = config["MQTT_USERNAME"] ?? "",
    Password = config["MQTT_PASSWORD"] ?? "",
};
var client = new HiveMQClient(options);
await client.ConnectAsync().ConfigureAwait(false);
builder.Services.AddSingleton(client);

builder.Services.AddSingleton<IAuthRepository, AuthRepository>();
builder.Services.AddSingleton<IRoomService, RoomService>();
builder.Services.AddSingleton<IMessageService, MessageService>();

var auth0Domain = config["AUTH0_DOMAIN"] ?? "";
var auth0Audience = config["AUTH0_AUDIENCE"] ?? "";
var auth0ClientId = config["AUTH0_CLIENT"] ?? "";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{auth0Domain}/";
        options.Audience = auth0Audience;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier,
            ValidAudience = auth0ClientId,
            ValidIssuer = auth0Domain,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero 
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();