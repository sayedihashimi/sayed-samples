using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorCSharpSupport.Pages.CSharp8;

public class CSharp8Model : PageModel
{
    public async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    /// <summary>
    /// Creates a readonly point - needed here since structs require separate definition
    /// </summary>
    public ReadonlyPoint CreateReadonlyPoint(int x, int y) => new(x, y);
}

/// <summary>
/// Example struct with readonly members (must be defined separately)
/// Demonstrates C# 8.0 readonly member feature
/// </summary>
public readonly struct ReadonlyPoint
{
    public readonly int X;
    public readonly int Y;

    public ReadonlyPoint(int x, int y)
    {
        X = x;
        Y = y;
    }

    public readonly double DistanceFromOrigin => Math.Sqrt(X * X + Y * Y);
    public readonly string ToCoordinateString() => $"Coordinates: ({X}, {Y})";
    public override readonly string ToString() => $"Point({X}, {Y})";
}

/// <summary>
/// Interface demonstrating default interface methods (C# 8.0 feature)
/// Must be defined separately from Razor page
/// </summary>
public interface ILogger
{
    void Log(string message);

    // Default interface method - provides implementation
    void LogWithTimestamp(string message)
    {
        Log($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
    }

    void LogError(string message)
    {
        Log($"ERROR: {message}");
    }
}

/// <summary>
/// Implementation of ILogger demonstrating default interface methods
/// </summary>
public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
        System.Diagnostics.Debug.WriteLine(message);
    }
}
