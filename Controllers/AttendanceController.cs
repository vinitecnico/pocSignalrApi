using Microsoft.AspNetCore.Mvc;

namespace PocSignalrApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly ILogger<AttendanceController> _logger;

    public AttendanceController(ILogger<AttendanceController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetAttendance")]
    public string Get()
    {
        return Guid.NewGuid().ToString();
    }

    [HttpDelete(Name = "removeAttendance")]
    public IActionResult Delete()
    {
        // if (todoItem == null)
        // {
        //     return NotFound();
        // }

        return NoContent();
    }
}
