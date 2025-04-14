using System.Security.Cryptography;
using System.Text;

namespace SecurityScanner.Razor.Samples;
public class WeakHashingExample
{
    // SECURITY ISSUE: Weak hash algorithm
    public string HashPassword(string input)
    {
        using var md5 = MD5.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        // Note: line of code below generates an IntelliSense Message when the file is open in VS
        //          Prefer static 'System.Security.Cryptography.MD5.HashData' method over 'ComputeHash'
        return Convert.ToBase64String(md5.ComputeHash(bytes));
    }
}
