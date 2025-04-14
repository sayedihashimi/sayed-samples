using Microsoft.AspNetCore.Mvc;

namespace SecurityScanner.Razor;
public class OpenRedirectController : Controller
{
    // SECURITY ISSUE: Open Redirect
    public IActionResult RedirectTo(string returnUrl)
    {
        return Redirect(returnUrl);
    }
}
