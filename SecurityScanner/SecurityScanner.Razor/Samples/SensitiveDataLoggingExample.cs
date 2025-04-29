using System;

namespace SecurityScanner.Razor.Samples;
public class SensitiveDataLoggingExample
{
    public void Login(string username, string password)
    {
        Console.WriteLine($"Login attempt: {username}, {password}");
    }
}
