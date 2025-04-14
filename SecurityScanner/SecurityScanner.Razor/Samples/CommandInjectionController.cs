using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace SecurityScanner.Razor;
public class CommandInjectionController : Controller
{
    // SECURITY ISSUE: Command Injection
    public IActionResult Run(string cmd)
    {
        Process.Start("cmd.exe", "/C " + cmd);
        return Ok();
    }
}
