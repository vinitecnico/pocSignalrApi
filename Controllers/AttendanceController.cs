using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PocSignalrApi.Hubs;

namespace PocSignalrApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly ILogger<AttendanceController> _logger;
    private readonly IHubContext<ChatHub> _hubContext;

    public AttendanceController(ILogger<AttendanceController> logger, IHubContext<ChatHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }

    [HttpGet("GetAttendance")]
    public string Get()
    {
        return Guid.NewGuid().ToString();
    }

    [HttpDelete("removeAttendance")]
    public IActionResult Delete()
    {
        // if (todoItem == null)
        // {
        //     return NotFound();
        // }

        return NoContent();
    }

    [HttpPost("message")]
    public async Task<IActionResult> SendMessage([FromBody] string message)
    {
        // await _chatHub.s
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
        return Ok();
    }
}
