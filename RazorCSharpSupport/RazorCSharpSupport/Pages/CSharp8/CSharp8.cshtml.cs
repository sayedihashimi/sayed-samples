using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;

namespace RazorCSharpSupport.Pages
{
    public class CSharp8Model : PageModel
    {
        public void OnGet()
        {
        }

        // C# 8.0: Asynchronous streams - IAsyncEnumerable
        public async IAsyncEnumerable<int> GenerateNumbersAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            for (int i = 1; i <= 10; i++)
            {
                // Simulate async work
                await Task.Delay(1, cancellationToken);
                yield return i;
            }
        }

        // C# 8.0: Asynchronous streams with filtering
        public async IAsyncEnumerable<int> GenerateEvenNumbersAsync(int count, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            int generated = 0;
            int current = 2;

            while (generated < count)
            {
                await Task.Delay(1, cancellationToken);
                yield return current;
                current += 2;
                generated++;
            }
        }

        // C# 8.0: Readonly members demonstration
        public string DemoReadonlyMembers()
        {
            var point = new ReadonlyMembersStruct(10, 20);
            return $"Distance from origin: {point.DistanceFromOrigin:F2}, ToString: {point}";
        }

        // C# 8.0: Enhanced interpolated verbatim strings
        public string InterpolatedVerbatimExample1()
        {
            string name = "World";
            int value = 42;
            
            // $@"" syntax (interpolated verbatim)
            return $@"Hello {name}!
This is a multi-line string
with value: {value}
and a backslash: \path\to\file";
        }

        public string InterpolatedVerbatimExample2()
        {
            string name = "C# 8";
            DateTime now = DateTime.Now;
            
            // @$"" syntax (verbatim interpolated) 
            return @$"Welcome to {name}!
Current time: {now:yyyy-MM-dd HH:mm:ss}
File path: C:\Users\{Environment.UserName}\Documents";
        }
    }

    // Supporting types for C# 8.0 demonstrations

    // Simple class for property pattern matching
    public class Person
    {
        public int Age { get; set; }
        public bool IsEmployed { get; set; }
        public string? Name { get; set; } // C# 8.0: Nullable reference types
    }

    // Record-like class with positional deconstruction for positional patterns
    public class Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        // Enable positional pattern matching with deconstruct
        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }

        public override string ToString() => $"({X}, {Y})";
    }

    // C# 8.0: Readonly members in struct
    public readonly struct ReadonlyMembersStruct
    {
        public readonly int X;
        public readonly int Y;

        public ReadonlyMembersStruct(int x, int y)
        {
            X = x;
            Y = y;
        }

        // C# 8.0: Readonly member methods
        public readonly double DistanceFromOrigin => Math.Sqrt(X * X + Y * Y);

        // This property could be readonly but we'll show mixed usage
        public readonly string Description => $"Point at ({X}, {Y})";

        // Non-readonly method for comparison (though this struct is entirely readonly)
        public override readonly string ToString() => $"ReadonlyPoint({X}, {Y})";
    }

    // C# 8.0: Default interface methods (requires separate interface)
    public interface ICalculator
    {
        int Add(int a, int b);
        
        // C# 8.0: Default interface method
        int Multiply(int a, int b) => a * b;
        
        // C# 8.0: Default interface method with logic
        double Average(params int[] numbers)
        {
            if (numbers.Length == 0) return 0;
            return numbers.Sum() / (double)numbers.Length;
        }
    }

    // Implementation of interface with default methods
    public class BasicCalculator : ICalculator
    {
        public int Add(int a, int b) => a + b;
        
        // Multiply and Average methods are inherited from interface default implementation
    }

    // Implementation that overrides default method
    public class AdvancedCalculator : ICalculator
    {
        public int Add(int a, int b) => a + b;

        // Override the default implementation
        public int Multiply(int a, int b)
        {
            // Could add logging, validation, etc.
            return a * b;
        }
    }
}
