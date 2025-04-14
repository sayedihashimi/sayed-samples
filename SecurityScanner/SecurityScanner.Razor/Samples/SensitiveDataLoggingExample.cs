using System;

namespace SecurityScanner.Razor.Samples;
public class SensitiveDataLoggingExample
{
    // SECURITY ISSUE: Logging sensitive data
    public void Login(string username, string password)
    {
        Console.WriteLine($"Login attempt: {username}, {password}");
    }
}
