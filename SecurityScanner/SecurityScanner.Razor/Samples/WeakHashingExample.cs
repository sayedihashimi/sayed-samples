using System.Security.Cryptography;
using System.Text;

namespace SecurityScanner.Razor;
public class WeakHashingExample
{
    // SECURITY ISSUE: Weak hash algorithm
    public string HashPassword(string input)
    {
        using var md5 = MD5.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(md5.ComputeHash(bytes));
    }
}
