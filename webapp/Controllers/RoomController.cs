using System.Text;
using HiveMQtt.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using server.Models;
using webapp.Errors;
using webapp.Hubs;
using webapp.Repositories;

namespace webapp.Controllers;

public class RoomController:AuthController
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRoomRepository _roomRepository;
    private readonly IMessageRepository _messageRepository;

    public RoomController(ILogger<HomeController> logger, IRoomRepository roomRepository, IMessageRepository messageRepository)
    {
        _logger = logger;
        _roomRepository = roomRepository;
        _messageRepository = messageRepository;
    }
    
    [Authorize]
    public async Task<IActionResult> SingleRoom(string id)
    {
        var idToken = await GetToken();
        if (idToken == null) throw new UnauthorizedException();
        
        ViewBag.RoomId = id;
        ViewBag.IdToken = idToken;
        var room = await _roomRepository.GetRoom(id, idToken);
        return View(room);
    }
    
    [Authorize]
    [Route("render-chat-message")]
    public ActionResult ActionMethodName([FromBody]MessageWithUser model)
    {
        return PartialView("ChatMessage", model);
    }
    
    [HttpPost("/api/rooms/{roomId}/messages")]
    public async Task<JsonResult> SendMessage(string roomId, [FromBody]SendMessageModel model)
    {
        var idToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        if (idToken == null) throw new UnauthorizedException();

        var message = await _messageRepository.SendMessage(roomId, model, idToken);
        return Json(message);
    }
}