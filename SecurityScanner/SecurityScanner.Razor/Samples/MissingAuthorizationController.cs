using Microsoft.AspNetCore.Mvc;

namespace SecurityScanner.Razor;
public class MissingAuthorizationController : Controller
{
    // SECURITY ISSUE: Missing [Authorize] on sensitive endpoint
    [HttpGet("admin/stats")]
    public IActionResult AdminStats()
    {
        return Ok("Top secret admin stats");
    }
}
