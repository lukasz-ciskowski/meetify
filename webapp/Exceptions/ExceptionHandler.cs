using Microsoft.AspNetCore.Diagnostics;
using webapp.Errors;

namespace webapp.Exceptions;

public class ExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ExceptionHandler> logger;
    public ExceptionHandler(ILogger<ExceptionHandler> logger)
    {
        this.logger = logger;
    }
    
    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is UnauthorizedException)
        {
            httpContext.Response.Redirect($"Account/Login/");
            return ValueTask.FromResult(true);
        }
        return ValueTask.FromResult(false);
    }
    
}