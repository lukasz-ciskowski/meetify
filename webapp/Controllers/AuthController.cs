using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using webapp.Errors;
using webapp.Models;

namespace webapp.Controllers;

public class AuthController:Controller
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var name = User.Identity?.Name;
        var image = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;
        
        ViewBag.User = new UserProfileViewModel()
        {
            Id = id ?? "",
            Name = name ?? "Anonymous",
            Email = email ?? "-",
            Image = image ?? ""
        };
        base.OnActionExecuting(filterContext);
    }
    
    public async Task<string> GetToken()
    {
        var token = await HttpContext.GetTokenAsync("id_token");
        if (token == null) throw new UnauthorizedException();
        return token;
    }
}