using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Repositories;
using server.Services;

namespace server.Controllers;

[Route("api/rooms/{roomId}/messages")]
[ApiController]
public class MessageController:ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly IAuthRepository _authRepository;
    
    public MessageController(IMessageService messageService, IAuthRepository authRepository)
    {
        this._messageService = messageService;
        this._authRepository = authRepository;
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SendMessage(string roomId, [FromBody]SendMessageModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();
        
        var message = new Message
        {
            SenderId = userId,
            MessageText = model.MessageText,
            RoomId = roomId,
            Timestamp = DateTime.Now
        };
        
        var result = await _messageService.AddMessage(message);
        
        return Ok(result);
        
    }
}