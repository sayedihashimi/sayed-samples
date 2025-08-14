using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RazorCSharpSupport.Pages.CSharp8;

/// <summary>
/// This file contains the same C# 8.0 code snippets from CSharp8.cshtml
/// for comparing language support between Razor and pure C# files
/// </summary>
public class CSharp8_CodeComparison
{
    // Supporting classes for demonstrations - same as in @functions block
    public class Person
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
    }

    public class Point3D
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Deconstruct(out int x, out int y, out int z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        public override string ToString() => $"({X}, {Y}, {Z})";
    }

    // Simple logger for demonstrations
    public class SimpleLogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
            System.Diagnostics.Debug.WriteLine(message);
        }
        
        public void LogWithTimestamp(string message)
        {
            Log($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
        }
    }

    // HTTP request simulation for demo purposes
    public class HttpRequest
    {
        public string Method { get; set; } = "GET";
        public string Path { get; set; } = "/";
        
        public HttpRequest(string method, string path)
        {
            Method = method;
            Path = path;
        }
    }

    /// <summary>
    /// 1. Switch Expressions Demo
    /// </summary>
    public void DemoSwitchExpressions()
    {
        var today = DateTime.Now.DayOfWeek;
        var dayType = today switch
        {
            DayOfWeek.Saturday or DayOfWeek.Sunday => "Weekend",
            DayOfWeek.Monday => "Monday Blues",
            DayOfWeek.Friday => "TGIF",
            _ => "Regular Weekday"
        };

        Console.WriteLine($"Today ({today}): {dayType}");

        // Switch expression with method call
        static string GetSeasonFromMonth(int month) => month switch
        {
            12 or 1 or 2 => "Winter",
            3 or 4 or 5 => "Spring", 
            6 or 7 or 8 => "Summer",
            9 or 10 or 11 => "Fall",
            _ => "Invalid Month"
        };

        Console.WriteLine($"Current Season: {GetSeasonFromMonth(DateTime.Now.Month)}");
    }

    /// <summary>
    /// 2. Property Patterns Demo
    /// </summary>
    public void DemoPropertyPatterns()
    {
        var people = new[]
        {
            new Person { Name = "Alice", Age = 10 },
            new Person { Name = "Bob", Age = 25 },
            new Person { Name = "Charlie", Age = 70 },
            new Person { Name = "Diana", Age = 16 }
        };

        string ClassifyPerson(Person person) => person switch
        {
            { Age: < 13 } => "Child",
            { Age: >= 13 and < 18 } => "Teenager", 
            { Age: >= 18 and < 65 } => "Adult",
            { Age: >= 65 } => "Senior",
            _ => "Unknown"
        };

        foreach (var person in people)
        {
            Console.WriteLine($"{person.Name} ({person.Age}): {ClassifyPerson(person)}");
        }
    }

    /// <summary>
    /// 3. Tuple Patterns Demo
    /// </summary>
    public void DemoTuplePatterns()
    {
        var coordinates = new[] { (0, 0), (0, 5), (3, 0), (2, 3), (-1, -1) };

        string DescribePoint((int x, int y) point) => point switch
        {
            (0, 0) => "Origin",
            (0, _) => "On Y-axis",
            (_, 0) => "On X-axis", 
            (_, _) when point.x == point.y => "On diagonal",
            (_, _) when point.x > 0 && point.y > 0 => "First quadrant",
            _ => "Other location"
        };

        foreach (var coord in coordinates)
        {
            Console.WriteLine($"({coord.Item1}, {coord.Item2}): {DescribePoint(coord)}");
        }
    }

    /// <summary>
    /// 4. Positional Patterns Demo
    /// </summary>
    public void DemoPositionalPatterns()
    {
        var points = new[]
        {
            new Point3D(0, 0, 0),
            new Point3D(1, 1, 1),
            new Point3D(5, 0, 0),
            new Point3D(0, 3, 4)
        };

        string AnalyzePoint(Point3D pt) => pt switch
        {
            (0, 0, 0) => "Origin in 3D",
            (var x, 0, 0) => $"On X-axis at {x}",
            (0, var y, 0) => $"On Y-axis at {y}",
            (0, 0, var z) => $"On Z-axis at {z}",
            (var x, var y, var z) when x == y && y == z => $"Cubic diagonal at ({x})",
            _ => $"General point {pt}"
        };

        foreach (var pt in points)
        {
            Console.WriteLine($"{pt}: {AnalyzePoint(pt)}");
        }
    }

    /// <summary>
    /// 5. Using Declarations Demo
    /// </summary>
    public void DemoUsingDeclarations()
    {
        string ProcessMemoryStream()
        {
            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream);
            
            writer.Write("Hello, C# 8!");
            writer.Flush();
            
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
            // All resources automatically disposed here
        }

        Console.WriteLine($"Processed Data: {ProcessMemoryStream()}");

        // Multiple using declarations
        string ProcessMultipleResources()
        {
            using var timer = new System.Timers.Timer(1000);
            using var httpClient = new HttpClient();
            using var stream = new MemoryStream();
            
            return $"Created {3} disposable resources that will be automatically cleaned up";
        }

        Console.WriteLine(ProcessMultipleResources());
    }

    /// <summary>
    /// 6. Static Local Functions Demo
    /// </summary>
    public void DemoStaticLocalFunctions()
    {
        int CalculateFactorial(int n)
        {
            if (n < 0) throw new ArgumentException("Negative numbers not allowed");
            
            static int Factorial(int x) => x <= 1 ? 1 : x * Factorial(x - 1);
            
            return Factorial(n);
        }

        static string FormatNumber(int number)
        {
            static string AddCommas(int num) => num.ToString("N0");
            static string GetOrdinal(int num) => num switch
            {
                1 => "1st",
                2 => "2nd", 
                3 => "3rd",
                _ => $"{num}th"
            };
            
            return $"{GetOrdinal(number)} number is {AddCommas(number)}";
        }

        for (int i = 0; i <= 6; i++)
        {
            Console.WriteLine($"Number: {i}, Factorial: {CalculateFactorial(i)}, Formatted: {FormatNumber(i + 1)}");
        }
    }

    /// <summary>
    /// 7. Indices and Ranges Demo
    /// </summary>
    public void DemoIndicesAndRanges()
    {
        var numbers = new[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        var fruits = new[] { "Apple", "Banana", "Cherry", "Date", "Elderberry", "Fig", "Grape" };

        // Index Operations
        Console.WriteLine($"Last element (^1): {numbers[^1]}");
        Console.WriteLine($"Second to last (^2): {numbers[^2]}");
        Console.WriteLine($"Third from end (^3): {numbers[^3]}");

        // Range Operations
        Console.WriteLine($"First 3 (0..3): [{string.Join(", ", numbers[0..3])}]");
        Console.WriteLine($"Last 3 (^3..): [{string.Join(", ", numbers[^3..])}]");
        Console.WriteLine($"Middle (2..^2): [{string.Join(", ", numbers[2..^2])}]");

        // Range with fruits
        var firstTwo = fruits[..2];
        var lastTwo = fruits[^2..];
        var middle = fruits[2..5];

        Console.WriteLine($"First two: {string.Join(", ", firstTwo)}");
        Console.WriteLine($"Last two: {string.Join(", ", lastTwo)}");
        Console.WriteLine($"Middle three: {string.Join(", ", middle)}");
    }

    /// <summary>
    /// 8. Null-Coalescing Assignment Demo
    /// </summary>
    public void DemoNullCoalescingAssignment()
    {
        string? message = null;
        string? name = "Alice";
        List<string>? items = null;

        // Assign default values using ??=
        message ??= "Default message";
        name ??= "Unknown";
        items ??= new List<string>();

        // Add some items
        items.Add("Item 1");
        items.Add("Item 2");

        Console.WriteLine($"Message: {message}");
        Console.WriteLine($"Name: {name}");
        Console.WriteLine($"Items: [{string.Join(", ", items)}]");

        // Configuration example
        Dictionary<string, string?> config = new()
        {
            {"timeout", null},
            {"retries", "3"},
            {"endpoint", null}
        };

        config["timeout"] ??= "30";
        config["retries"] ??= "5"; // Won't override existing value
        config["endpoint"] ??= "https://api.example.com";

        foreach (var kvp in config)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }

    /// <summary>
    /// 9. Enhanced Interpolated Verbatim Strings Demo
    /// </summary>
    public void DemoEnhancedInterpolatedVerbatimStrings()
    {
        var userName = "Alice";
        var filePath = @"C:\Users\Alice\Documents";

        // Both syntaxes work: $@"" and @$""
        var path1 = $@"Welcome {userName}! 
Your files are at: {filePath}\data
Use backslashes: \n \t \r";

        var path2 = @$"Alternative syntax:
User: {userName}
Path: {filePath}\backup
Special chars: \\ \n \t";

        var multilineQuery = $@"
SELECT * 
FROM Users 
WHERE Name = '{userName}' 
  AND Path LIKE '{filePath}%'
  AND Created > '2023-01-01'";

        Console.WriteLine("Dollar-At-Quote Syntax:");
        Console.WriteLine(path1);
        Console.WriteLine("\nAt-Dollar-Quote Syntax:");
        Console.WriteLine(path2);
        Console.WriteLine("\nSQL Query Example:");
        Console.WriteLine(multilineQuery);
    }

    /// <summary>
    /// 10. Asynchronous Streams Demo
    /// </summary>
    public async Task DemoAsynchronousStreams()
    {
        // Helper methods for demonstrations
        async IAsyncEnumerable<int> GetAsyncNumbers()
        {
            for (int i = 1; i <= 5; i++)
            {
                await Task.Delay(10);
                yield return i * 10;
            }
        }

        async IAsyncEnumerable<string> GetAsyncMessages()
        {
            var messages = new[] { "Loading...", "Processing...", "Almost done...", "Complete!" };
            
            foreach (var message in messages)
            {
                await Task.Delay(5);
                yield return $"{DateTime.Now:HH:mm:ss} - {message}";
            }
        }

        // Using local async methods
        var asyncNumbers = await GetAsyncNumbers().ToListAsync();
        var asyncMessages = await GetAsyncMessages().ToListAsync();

        Console.WriteLine("Async Numbers:");
        foreach (var number in asyncNumbers)
        {
            Console.WriteLine($"Number: {number}");
        }

        Console.WriteLine("\nAsync Messages:");
        foreach (var msg in asyncMessages)
        {
            Console.WriteLine(msg);
        }
    }

    /// <summary>
    /// 11. Nullable Reference Types Demo
    /// </summary>
    public void DemoNullableReferenceTypes()
    {
        // These will generate compiler warnings if used incorrectly
        string nonNullable = "Hello";
        string? nullable = null;

        // Safe operations
        var length1 = nonNullable.Length; // Safe
        var length2 = nullable?.Length ?? 0; // Safe with null-conditional

        // Examples of nullable-aware code
        string ProcessString(string? input)
        {
            if (string.IsNullOrEmpty(input))
                return "Empty or null input";
            
            return $"Processed: {input.ToUpper()}"; // Safe after null check
        }

        string?[] nullableArray = { "Hello", null, "World", null, "!" };
        var processedStrings = nullableArray
            .Where(s => s != null)
            .Select(s => s!.ToUpper())
            .ToArray();

        Console.WriteLine("Nullable Analysis Results:");
        Console.WriteLine($"Non-nullable length: {length1}");
        Console.WriteLine($"Nullable length (safe): {length2}");
        Console.WriteLine($"Processed null: {ProcessString(null)}");
        Console.WriteLine($"Processed valid: {ProcessString("test")}");
        Console.WriteLine($"Filtered array: [{string.Join(", ", processedStrings)}]");
    }

    /// <summary>
    /// 12. Readonly Members Demo (using struct from code-behind)
    /// </summary>
    public void DemoReadonlyMembers()
    {
        // Note: Using readonly struct from code-behind since structs need separate definition
        var model = new CSharp8Model();
        var point = model.CreateReadonlyPoint(10, 20);
        var distance = point.DistanceFromOrigin;
        var description = point.ToString();

        Console.WriteLine($"Point: {point}");
        Console.WriteLine($"Distance from origin: {distance:F2}");
        Console.WriteLine($"Description: {description}");
        Console.WriteLine("Note: Readonly members must be defined in code-behind or separate classes.");
    }

    /// <summary>
    /// 13. Default Interface Methods Demo
    /// </summary>
    public void DemoDefaultInterfaceMethods()
    {
        var logger = new SimpleLogger();
        var logMessage = "Test message";
        
        // Simple logging demonstrations
        logger.Log(logMessage);
        logger.LogWithTimestamp(logMessage);
        
        Console.WriteLine("Logger Output: Check console for logged messages");
        Console.WriteLine("Note: Default interface methods must be defined in separate interface files.");

        // For true default interface methods, see code-behind examples
        var consoleLogger = new ConsoleLogger();
        consoleLogger.Log("Testing default interface methods");
        ((ILogger)consoleLogger).LogWithTimestamp("Using default implementation");
    }

    /// <summary>
    /// Main method to run all demonstrations
    /// </summary>
    public async Task RunAllDemonstrations()
    {
        Console.WriteLine("=== C# 8.0 Features Demonstration ===\n");

        Console.WriteLine("1. Switch Expressions:");
        DemoSwitchExpressions();
        Console.WriteLine();

        Console.WriteLine("2. Property Patterns:");
        DemoPropertyPatterns();
        Console.WriteLine();

        Console.WriteLine("3. Tuple Patterns:");
        DemoTuplePatterns();
        Console.WriteLine();

        Console.WriteLine("4. Positional Patterns:");
        DemoPositionalPatterns();
        Console.WriteLine();

        Console.WriteLine("5. Using Declarations:");
        DemoUsingDeclarations();
        Console.WriteLine();

        Console.WriteLine("6. Static Local Functions:");
        DemoStaticLocalFunctions();
        Console.WriteLine();

        Console.WriteLine("7. Indices and Ranges:");
        DemoIndicesAndRanges();
        Console.WriteLine();

        Console.WriteLine("8. Null-Coalescing Assignment:");
        DemoNullCoalescingAssignment();
        Console.WriteLine();

        Console.WriteLine("9. Enhanced Interpolated Verbatim Strings:");
        DemoEnhancedInterpolatedVerbatimStrings();
        Console.WriteLine();

        Console.WriteLine("10. Asynchronous Streams:");
        await DemoAsynchronousStreams();
        Console.WriteLine();

        Console.WriteLine("11. Nullable Reference Types:");
        DemoNullableReferenceTypes();
        Console.WriteLine();

        Console.WriteLine("12. Readonly Members:");
        DemoReadonlyMembers();
        Console.WriteLine();

        Console.WriteLine("13. Default Interface Methods:");
        DemoDefaultInterfaceMethods();
        Console.WriteLine();

        Console.WriteLine("=== All C# 8.0 Features Demonstrated ===");
    }

    public class SayedHa
    {
        public readonly int Id = 100;
    }
    public void TestReadonly()
    {
        var sayed = new SayedHa();
        Console.WriteLine($"Readonly Id: {sayed.Id}");
    }
    public class OtherClass
    {
        public readonly SayedHa sayed = new SayedHa();
    }
}
