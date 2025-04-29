using Microsoft.AspNetCore.Mvc;

namespace SecurityScanner.Razor.Samples;
public class UnvalidatedInputController : Controller
{
    public IActionResult Search(string q)
    {
        return Content("Searching for: " + q); // Potential XSS
    }
}
