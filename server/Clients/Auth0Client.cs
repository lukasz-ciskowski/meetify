using System.Net.Http.Headers;
using System.Text.Json;
using server.Models;

namespace server.Clients;

public class Auth0Client:HttpClient
{
    private readonly IConfiguration _configuration;
    public Auth0Client(IConfiguration configuration) : base()
    {
        var apiUrl = $"https://{configuration["AUTH0_DOMAIN"]}/";
        this.BaseAddress = new Uri(apiUrl);
        this._configuration = configuration;
    }

    public Auth0Client AddAuthHeader(string accessToken)
    {
        this.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return this;
    }
}
