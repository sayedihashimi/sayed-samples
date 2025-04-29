using Microsoft.AspNetCore.Mvc;

namespace SecurityScanner.Razor.Samples;
public class OpenRedirectController : Controller
{
    public IActionResult RedirectTo(string returnUrl)
    {
        return Redirect(returnUrl);
    }
}
