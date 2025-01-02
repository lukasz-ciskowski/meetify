using server.Models;

namespace server.Repositories;

public interface IAuthRepository
{
    public Task<AuthResponse> Authorize();
    public Task<Auth0User> GetUser(string userId);
    public Task<List<Auth0User?>> GetUsers(List<string> userIds);
}