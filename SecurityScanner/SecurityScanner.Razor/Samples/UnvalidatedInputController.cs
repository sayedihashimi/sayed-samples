using Microsoft.AspNetCore.Mvc;

namespace SecurityScanner.Razor.Samples;
public class UnvalidatedInputController : Controller
{
    // SECURITY ISSUE: Unvalidated input used directly
    public IActionResult Search(string q)
    {
        return Content("Searching for: " + q); // Potential XSS
    }
}
