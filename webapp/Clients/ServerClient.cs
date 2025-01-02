using System.Net;
using System.Net.Http.Headers;
using webapp.Errors;
using webapp.Exceptions;

namespace webapp.Clients;

public class ServerClient : HttpClient
{
    
    public ServerClient(IConfiguration configuration) : base()
    {
        var apiUrl = configuration["Services:Server"]!;
        this.BaseAddress = new Uri(apiUrl);
    }

    public ServerClient AddAuthHeader(string accessToken)
    {
        this.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return this;
    }
    
    public async Task<HttpResponseMessage> PerformRequest(Func<Task<HttpResponseMessage>> func)
    {
 
        var response = await func();
        if (response.IsSuccessStatusCode) return response;
        
        if (response.StatusCode == HttpStatusCode.Unauthorized) throw new UnauthorizedException();
        if (response.StatusCode == HttpStatusCode.BadRequest) throw new BadRequestException();
        throw new InternalException();
    }
}