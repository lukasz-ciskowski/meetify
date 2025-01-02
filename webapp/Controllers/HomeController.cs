using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using server.Models;
using webapp.Errors;
using webapp.Exceptions;
using webapp.Models;
using webapp.Repositories;

namespace webapp.Controllers;

public class HomeController : AuthController
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRoomRepository _roomRepository;

    public HomeController(ILogger<HomeController> logger, IRoomRepository roomRepository)
    {
        _logger = logger;
        _roomRepository = roomRepository;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var idToken = await GetToken();
        if (idToken == null) throw new UnauthorizedException();
        
        var rooms = await _roomRepository.GetRooms(idToken);
        return View(rooms);   
    }
    
    [Authorize]
    [Route("add-room")]
    public IActionResult AddRoomForm(string error)
    {
        if (!string.IsNullOrEmpty(error))
        {
            ViewBag.ErrorMessage = error;
        }
        return View();
    }
    
    [Authorize]
    public async Task<IActionResult> CreateNewRoomMethod(CreateRoomModel room)
    {
        var idToken = await GetToken();
        if (idToken == null) throw new UnauthorizedException();
        try
        {
            await _roomRepository.AddRoom(room, idToken);
        }
        catch (Exception e)
        {
            if (e is BadRequestException) return RedirectToAction("AddRoomForm", new { error = "Invalid room data" });
            throw;
        }
        return RedirectToAction("Index");
    }
}