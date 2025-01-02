using System.Net.Http.Headers;
using server.Clients;
using server.Models;

namespace server.Repositories;

public class AuthRepository:IAuthRepository
{
    private readonly IConfiguration _configuration;
    
    private AuthResponse? _cachedAuthResponse;
    // hashmap
    private readonly Dictionary<string, Auth0User> _cachedUsers = new();
    
    public AuthRepository(IConfiguration configuration)
    {   
        _configuration = configuration;
    }
    
    public async Task<AuthResponse> Authorize()
    {   
        if (_cachedAuthResponse != null)
        {
            return _cachedAuthResponse;
        }
        
        using var client = new Auth0Client(_configuration);
        
        var clientId = _configuration["AUTH0_CLIENT"] ?? "";
        var clientSecret = _configuration["AUTH0_CLIENT_SECRET"] ?? "";
        var audience = _configuration["AUTH0_AUDIENCE"] ?? "";
        var grantType = "client_credentials";

        var request = new HttpRequestMessage(HttpMethod.Post, "oauth/token");
        
        request.Content = JsonContent.Create(new
        {
            client_id = clientId,
            client_secret = clientSecret,
            audience = audience,
            grant_type = grantType
        });
        
        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to authorize");
        }
        
        var jsonResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

        _cachedAuthResponse = jsonResponse ?? throw new Exception("Failed to authorize");
        
        return jsonResponse;
    }
    
    public async Task<Auth0User> GetUser(string userId)
    {
        if (_cachedUsers.ContainsKey(userId))
        {
            return _cachedUsers[userId];
        }
        
        var authResponse = await Authorize();
        
        using var client = new Auth0Client(_configuration);
        
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/v2/users/{userId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResponse.access_token);
        
        var response = await client.SendAsync(request);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to get user");
        }
        
        var user = await response.Content.ReadFromJsonAsync<Auth0User>();
        // manually set the UserId property
        user.UserId = userId;
        
        _cachedUsers[userId] = user ?? throw new Exception("Failed to get user");
        
        return user;
    }
    
    public async Task<List<Auth0User?>> GetUsers(List<string> userIds)
    {
        var userTasks = userIds.Select(userId => GetUser(userId));
        var users = await Task.WhenAll(userTasks);
        return users.ToList();
    }
}