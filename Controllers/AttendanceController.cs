using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using PocSignalrApi.Hubs;

namespace PocSignalrApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AttendanceController : ControllerBase
{
    private const string employeeListCacheKey = "test";
    private readonly ILogger<AttendanceController> _logger;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IMemoryCache _memoryCache;

    public AttendanceController(ILogger<AttendanceController> logger, IHubContext<ChatHub> hubContext, IMemoryCache memoryCache)
    {
        _logger = logger;
        _hubContext = hubContext;
        _memoryCache = memoryCache;
    }

    [HttpGet("GetAttendance")]
    public string Get()
    {

        if (_memoryCache.TryGetValue(employeeListCacheKey, out string? query))
            return query;

        var cacheOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

        query = Guid.NewGuid().ToString();
        _memoryCache.Set(employeeListCacheKey, query, cacheOptions);
        return query;
    }

    [HttpDelete("removeAttendance")]
    public IActionResult Delete()
    {
        // if (todoItem == null)
        // {
        //     return NotFound();
        // }
        _memoryCache.Remove(employeeListCacheKey);
        return NoContent();
    }

    [HttpPost("message")]
    public async Task<IActionResult> SendMessage([FromBody] string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);

        var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(1024);

        _memoryCache.Set(employeeListCacheKey, message, cacheEntryOptions);

        return Ok();
    }
}
