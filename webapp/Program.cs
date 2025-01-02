using Auth0.AspNetCore.Authentication;
using HiveMQtt.Client;
using HiveMQtt.Client.Options;
using webapp.Clients;
using webapp.Exceptions;
using webapp.Hubs;
using webapp.Repositories;
using webapp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddSignalR();

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var auth0Domain = config["AUTH0_DOMAIN"] ?? "";
var auth0ClientId = config["AUTH0_CLIENT"] ?? "";

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

builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = auth0Domain;
    options.ClientId = auth0ClientId;
});
builder.Services.AddSingleton<IRoomRepository, RoomRepository>().AddSingleton<IMessageRepository, MessageRepository>();
builder.Services.AddSingleton<IMessageService, MessageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseExceptionHandler(_ => {});

app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<ChatHub>("/chat");

var mqttBroker = new MqttBroker(client, app.Services.GetRequiredService<IMessageService>());

app.Run();