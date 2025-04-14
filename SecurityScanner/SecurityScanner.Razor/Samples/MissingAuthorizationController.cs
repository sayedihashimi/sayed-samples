using Microsoft.AspNetCore.Mvc;

namespace SecurityScanner.Razor.Samples;
public class MissingAuthorizationController : Controller
{
    // SECURITY ISSUE: Missing [Authorize] on sensitive endpoint
    //  TODO: How to know if an endpoint is "sensitive"?
    [HttpGet("admin/stats")]
    public IActionResult AdminStats()
    {
        return Ok("Top secret admin stats");
    }
}
