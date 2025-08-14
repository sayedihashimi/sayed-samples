using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RazorCSharpSupport.Pages.CSharp8
{
    public class CSharp8Examples
    {
        // Helper classes for demonstrations
        public class Person
        {
            public int Age { get; set; }
            public bool IsEmployed { get; set; }
        }

        public struct Point
        {
            public int X { get; }
            public int Y { get; }

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            // Deconstruction method for positional patterns
            public void Deconstruct(out int x, out int y)
            {
                x = X;
                y = Y;
            }
        }

        // Readonly struct with readonly members
        public readonly struct ReadonlyPoint
        {
            public int X { get; }
            public int Y { get; }

            public ReadonlyPoint(int x, int y)
            {
                X = x;
                Y = y;
            }

            // C# 8.0: Readonly member
            public readonly double DistanceFromOrigin()
            {
                return Math.Sqrt(X * X + Y * Y);
            }

            // Non-readonly member for comparison
            public override string ToString()
            {
                return $"({X}, {Y})";
            }
        }

        // C# 8.0: Switch expressions
        public static string GetDayType(DayOfWeek day) => day switch
        {
            DayOfWeek.Saturday or DayOfWeek.Sunday => "Weekend",
            DayOfWeek.Monday => "Monday Blues",
            DayOfWeek.Friday => "TGIF",
            _ => "Weekday"
        };

        // C# 8.0: Property patterns
        public static string ClassifyPerson(Person person) => person switch
        {
            { Age: < 13 } => "Child",
            { Age: >= 13 and < 20 } => "Teenager",
            { Age: >= 20 and < 65, IsEmployed: true } => "Working Adult",
            { Age: >= 65 } => "Senior",
            _ => "Adult"
        };

        // C# 8.0: Tuple patterns
        public static string AnalyzeCoordinates((int x, int y) point) => point switch
        {
            (0, 0) => "Origin",
            (0, _) => "On Y-axis",
            (_, 0) => "On X-axis",
            (var x, var y) when x == y => "Diagonal",
            (var x, var y) when x > 0 && y > 0 => "First quadrant",
            _ => "Other quadrant"
        };

        // C# 8.0: Positional patterns (with custom types)
        public static string ClassifyPoint(Point point) => point switch
        {
            (0, 0) => "Origin",
            (var x, 0) => $"On X-axis at {x}",
            (0, var y) => $"On Y-axis at {y}",
            (var x, var y) when x == y => $"Diagonal at ({x}, {y})",
            _ => $"Point at ({point.X}, {point.Y})"
        };

        // C# 8.0: Using declarations (automatic disposal)
        public static string ReadFromMemoryStream()
        {
            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream);
            writer.Write("Hello C# 8!");
            writer.Flush();
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        // C# 8.0: Static local functions
        public static int CalculateFactorial(int n)
        {
            static int Factorial(int x, int accumulator = 1)
            {
                return x <= 1 ? accumulator : Factorial(x - 1, x * accumulator);
            }
            return n < 0 ? throw new ArgumentException("Negative numbers not allowed") : Factorial(n);
        }

        // C# 8.0: Indices and ranges
        public static void DemonstrateIndicesAndRanges()
        {
            string[] fruits = { "apple", "banana", "cherry", "date", "elderberry" };

            // Index from end
            string lastFruit = fruits[^1];
            string secondLastFruit = fruits[^2];

            // Range operations
            string[] firstThree = fruits[0..3];
            string[] lastTwo = fruits[^2..];
            string[] middle = fruits[1..^1];

            Console.WriteLine($"Last fruit: {lastFruit}");
            Console.WriteLine($"Second last fruit: {secondLastFruit}");
            Console.WriteLine($"First three: [{string.Join(", ", firstThree)}]");
            Console.WriteLine($"Last two: [{string.Join(", ", lastTwo)}]");
            Console.WriteLine($"Middle fruits: [{string.Join(", ", middle)}]");
        }

        // C# 8.0: Null-coalescing assignment
        public static void DemonstrateNullCoalescingAssignment()
        {
            string? nullableString = null;
            nullableString ??= "Default value assigned";

            List<string>? nullableList = null;
            nullableList ??= new List<string> { "item1", "item2" };

            Console.WriteLine($"Nullable string: {nullableString}");
            Console.WriteLine($"Nullable list count: {nullableList.Count}");
        }

        // C# 8.0: Asynchronous streams
        public static async IAsyncEnumerable<int> GenerateNumbersAsync()
        {
            for (int i = 1; i <= 5; i++)
            {
                await Task.Delay(100); // Simulate async work
                yield return i;
            }
        }

        public static async IAsyncEnumerable<int> GenerateEvenNumbersAsync(int count)
        {
            int current = 0;
            for (int i = 0; i < count; i++)
            {
                current += 2;
                await Task.Delay(50); // Simulate async work
                yield return current;
            }
        }

        // C# 8.0: Enhanced interpolated verbatim strings
        public static string InterpolatedVerbatimExample1()
        {
            string name = "World";
            string path = @"C:\Users\";
            return $@"Hello {name}!
Path: {path}
Line breaks preserved.";
        }

        public static string InterpolatedVerbatimExample2()
        {
            string name = "World";
            string path = @"C:\Users\";
            return @$"Hello {name}!
Path: {path}
Line breaks preserved.";
        }

        // C# 8.0: Readonly members demonstration
        public static string DemoReadonlyMembers()
        {
            var point = new ReadonlyPoint(3, 4);
            return $"Point {point} is {point.DistanceFromOrigin():F2} units from origin";
        }

        // Demonstration method that exercises all features
        public static async Task RunAllDemonstrations()
        {
            Console.WriteLine("=== C# 8.0 Feature Demonstrations ===\n");

            // Switch expressions
            Console.WriteLine("🔄 Switch Expressions:");
            Console.WriteLine($"Saturday: {GetDayType(DayOfWeek.Saturday)}");
            Console.WriteLine($"Monday: {GetDayType(DayOfWeek.Monday)}");
            Console.WriteLine($"Friday: {GetDayType(DayOfWeek.Friday)}");
            Console.WriteLine($"Wednesday: {GetDayType(DayOfWeek.Wednesday)}");
            Console.WriteLine();

            // Property patterns
            Console.WriteLine("🎯 Property Patterns:");
            var person1 = new Person { Age = 10, IsEmployed = false };
            var person2 = new Person { Age = 25, IsEmployed = true };
            var person3 = new Person { Age = 70, IsEmployed = false };

            Console.WriteLine($"Age {person1.Age}, Employed: {person1.IsEmployed}: {ClassifyPerson(person1)}");
            Console.WriteLine($"Age {person2.Age}, Employed: {person2.IsEmployed}: {ClassifyPerson(person2)}");
            Console.WriteLine($"Age {person3.Age}, Employed: {person3.IsEmployed}: {ClassifyPerson(person3)}");
            Console.WriteLine();

            // Tuple patterns
            Console.WriteLine("📐 Tuple Patterns:");
            Console.WriteLine($"(0, 0): {AnalyzeCoordinates((0, 0))}");
            Console.WriteLine($"(5, 5): {AnalyzeCoordinates((5, 5))}");
            Console.WriteLine($"(3, 0): {AnalyzeCoordinates((3, 0))}");
            Console.WriteLine($"(2, 8): {AnalyzeCoordinates((2, 8))}");
            Console.WriteLine();

            // Positional patterns
            Console.WriteLine("📍 Positional Patterns:");
            var point1 = new Point(0, 0);
            var point2 = new Point(5, 5);
            var point3 = new Point(3, 7);

            Console.WriteLine($"Point (0, 0): {ClassifyPoint(point1)}");
            Console.WriteLine($"Point (5, 5): {ClassifyPoint(point2)}");
            Console.WriteLine($"Point (3, 7): {ClassifyPoint(point3)}");
            Console.WriteLine();

            // Using declarations
            Console.WriteLine("🔧 Using Declarations:");
            string memoryContent = ReadFromMemoryStream();
            Console.WriteLine($"Memory stream content: \"{memoryContent}\"");
            Console.WriteLine("Resources automatically disposed at end of scope");
            Console.WriteLine();

            // Static local functions
            Console.WriteLine("⚡ Static Local Functions:");
            int factorial5 = CalculateFactorial(5);
            Console.WriteLine($"Factorial of 5: {factorial5}");
            Console.WriteLine("Static local function prevents accidental captures");
            Console.WriteLine();

            // Indices and ranges
            Console.WriteLine("🔢 Indices and Ranges:");
            DemonstrateIndicesAndRanges();
            Console.WriteLine();

            // Null-coalescing assignment
            Console.WriteLine("❓ Null-Coalescing Assignment:");
            DemonstrateNullCoalescingAssignment();
            Console.WriteLine();

            // Asynchronous streams
            Console.WriteLine("🌊 Asynchronous Streams:");
            var numbers = await GenerateNumbersAsync().ToListAsync();
            var evenNumbers = await GenerateEvenNumbersAsync(10).ToListAsync();
            Console.WriteLine($"Generated numbers: [{string.Join(", ", numbers)}]");
            Console.WriteLine($"Even numbers (first 10): [{string.Join(", ", evenNumbers)}]");
            Console.WriteLine();

            // Readonly members
            Console.WriteLine("💎 Readonly Members:");
            Console.WriteLine($"Readonly struct demo: {DemoReadonlyMembers()}");
            Console.WriteLine();

            // Enhanced interpolated verbatim strings
            Console.WriteLine("🔤 Enhanced Interpolated Verbatim Strings:");
            Console.WriteLine($"$@\"\" syntax:\n{InterpolatedVerbatimExample1()}");
            Console.WriteLine($"@$\"\" syntax:\n{InterpolatedVerbatimExample2()}");
            Console.WriteLine();

            // Inline expression examples
            Console.WriteLine("🚀 Inline Expression Examples:");
            
            // Switch expressions in expressions
            Console.WriteLine($"Grade 85: {85 switch { >= 90 => "A", >= 80 => "B", >= 70 => "C", >= 60 => "D", _ => "F" }}");
            Console.WriteLine($"Size Large: {"Large" switch { "Small" => "S", "Medium" => "M", "Large" => "L", "XLarge" => "XL", _ => "Unknown" }}");
            Console.WriteLine($"Boolean true: {true switch { true => "Yes", false => "No" }}");

            // Property patterns in expressions
            var now = DateTime.Now;
            Console.WriteLine($"Current time: {now switch { { DayOfWeek: DayOfWeek.Saturday or DayOfWeek.Sunday } => "Weekend", { Hour: >= 9 and <= 17 } => "Business Hours", _ => "Other Time" }}");
            Console.WriteLine($"String analysis: {"Hello World" switch { { Length: > 10 } => "Long string", { Length: <= 5 } => "Short string", _ => "Medium string" }}");

            // Ranges and indices in expressions
            Console.WriteLine($"Last character of 'Hello': '{"Hello"[^1]}'");
            Console.WriteLine($"First 3 characters of 'Programming': \"{"Programming"[0..3]}\"");
            Console.WriteLine($"Skip first and last of 'Wonderful': \"{"Wonderful"[1..^1]}\"");
            Console.WriteLine($"From index 2 to end of 'Amazing': \"{"Amazing"[2..]}\"");
        }
    }

    // Extension method to convert IAsyncEnumerable to List
    public static class AsyncEnumerableExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source)
        {
            var list = new List<T>();
            await foreach (var item in source)
            {
                list.Add(item);
            }
            return list;
        }
    }
}
