using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace SecurityScanner.Razor.Samples;
public class CommandInjectionController : Controller
{
    public IActionResult Run(string cmd)
    {
        Process.Start("cmd.exe", "/C " + cmd);
        return Ok();
    }
}
