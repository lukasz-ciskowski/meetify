using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Repositories;
using server.Services;

namespace server.Controllers;

[Route("api/rooms")]
[ApiController]
public class RoomController: ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly IAuthRepository _authRepository;
    
    public RoomController(IRoomService roomService, IAuthRepository authRepository)
    {
        this._roomService = roomService;
        this._authRepository = authRepository;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetRooms()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }
        
        var rooms = await _roomService.GetRooms(userId);
        
        // var auth = await _authRepository.Authorize();
        // Console.WriteLine(auth.access_token);
        
        return Ok(rooms);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddRoom(CreateRoomModel roomModel)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId == null) return Unauthorized();
        
        var room = new Room
        {
            Title = roomModel.Title,
            Description = roomModel.Description,
            Visibility = roomModel.Visibility,
            CreatorId = userId,
        };
        
        var result = await _roomService.AddRoom(room);
        return Ok(result);
    }
    
    [HttpGet("{roomId}")]
    [Authorize]
    public async Task<IActionResult> GetRoom(string roomId)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();
        
        var room = await _roomService.GetRoomWithMessages(roomId, userId);
        
        return Ok(room);
    }
}