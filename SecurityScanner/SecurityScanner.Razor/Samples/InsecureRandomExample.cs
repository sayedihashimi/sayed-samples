using System;

namespace SecurityScanner.Razor;
public class InsecureRandomExample
{
    // SECURITY ISSUE: Insecure randomness
    public string GenerateResetCode()
    {
        var rand = new Random();
        return rand.Next(100000, 999999).ToString();
    }
}
