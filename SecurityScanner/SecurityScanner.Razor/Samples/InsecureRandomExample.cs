using System;

namespace SecurityScanner.Razor.Samples;
public class InsecureRandomExample {
    private static readonly HashSet<string> IssuedResetCodes = [];

    public string GenerateResetCode(string userEmail) {
        var rand = new Random();
        var resetCode = rand.Next(100000, 999999).ToString();

        // Simulate issuing a reset token (insecure due to predictable output)
        IssuedResetCodes.Add(resetCode);
        Console.WriteLine($"Issued reset code for {userEmail}: {resetCode}");

        return resetCode;

    }
}