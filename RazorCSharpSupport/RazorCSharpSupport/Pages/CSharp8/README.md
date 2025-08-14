# C# 8.0 Features in Razor Pages

This folder contains comprehensive demonstrations of C# 8.0 features within Razor Pages, showing both compatibility and limitations.

## 📁 Files

- **CSharp8.cshtml** - Main Razor page demonstrating all compatible C# 8.0 features
- **CSharp8.cshtml.cs** - PageModel with supporting classes and async stream implementations
- **README.md** - This documentation file

## ✅ Fully Compatible Features

### 1. Switch Expressions
- **Description**: More concise and expression-based switch syntax
- **Razor Compatibility**: ✅ Full support in both code blocks and inline expressions
- **Example**: `day switch { DayOfWeek.Saturday => "Weekend", _ => "Weekday" }`

### 2. Property Patterns
- **Description**: Pattern matching on object properties with complex conditions
- **Razor Compatibility**: ✅ Works perfectly with object property matching
- **Example**: `person switch { { Age: < 13 } => "Child", { Age: >= 65 } => "Senior" }`

### 3. Tuple Patterns
- **Description**: Pattern matching on tuple values and deconstruction
- **Razor Compatibility**: ✅ Complete support for tuple pattern matching
- **Example**: `point switch { (0, 0) => "Origin", (0, _) => "Y-axis" }`

### 4. Positional Patterns
- **Description**: Pattern matching with positional deconstruction
- **Razor Compatibility**: ✅ Works with custom types that implement Deconstruct
- **Example**: Custom Point class with Deconstruct method

### 5. Using Declarations
- **Description**: Automatic resource disposal without explicit using blocks
- **Razor Compatibility**: ✅ Automatic disposal works as expected
- **Example**: `using var stream = new MemoryStream();`

### 6. Static Local Functions
- **Description**: Local functions that cannot capture variables (better performance)
- **Razor Compatibility**: ✅ Full support, prevents accidental captures
- **Example**: `static int Factorial(int x) => x <= 1 ? 1 : x * Factorial(x - 1);`

### 7. Indices and Ranges
- **Description**: New syntax for array element access and slicing
- **Razor Compatibility**: ✅ All operators work (^, .., Range, Index)
- **Examples**: 
  - `array[^1]` (last element)
  - `array[0..3]` (first three elements)
  - `array[^2..]` (last two elements)

### 8. Null-Coalescing Assignment
- **Description**: Assign value only if left operand is null
- **Razor Compatibility**: ✅ ??= operator works in all contexts
- **Example**: `value ??= "default";`

### 9. Asynchronous Streams
- **Description**: IAsyncEnumerable for async iteration
- **Razor Compatibility**: ✅ Full support with await and ToListAsync()
- **Example**: `await foreach` loops and `IAsyncEnumerable<T>` return types

### 10. Enhanced Interpolated Verbatim Strings
- **Description**: Combine interpolation with verbatim strings in any order
- **Razor Compatibility**: ✅ Both $@"" and @$"" syntax supported
- **Example**: `$@"Multi-line {variable} with \backslashes"`

## 🔄 Limited/Special Context Features

### 1. Readonly Members
- **Description**: Mark individual struct members as readonly
- **Razor Compatibility**: ⚠️ Requires struct definition in code-behind or external class
- **Limitation**: Cannot define structs inline in Razor code blocks
- **Workaround**: Define in PageModel or separate class file

### 2. Default Interface Methods
- **Description**: Interfaces can provide default implementations
- **Razor Compatibility**: ⚠️ Must be defined in separate interface files
- **Limitation**: Cannot define interfaces inline in Razor
- **Workaround**: Define interfaces in code-behind or separate files

### 3. Nullable Reference Types
- **Description**: Compile-time null safety for reference types
- **Razor Compatibility**: ⚠️ Enabled project-wide via `<Nullable>enable</Nullable>`
- **Notes**: Warnings appear in build output, not in Razor runtime

## ❌ Incompatible/Impractical Features

### 1. Disposable Ref Structs
- **Description**: ref structs that implement IDisposable pattern
- **Razor Compatibility**: ❌ Cannot demonstrate effectively in Razor context
- **Reason**: ref struct limitations in method calls and storage

### 2. Unmanaged Constructed Types
- **Description**: Generic types with unmanaged type constraints
- **Razor Compatibility**: ❌ Requires unsafe code context
- **Reason**: Unsafe code not suitable for Razor pages

### 3. Stackalloc in Nested Expressions
- **Description**: Use stackalloc in more expression contexts
- **Razor Compatibility**: ❌ Requires unsafe context
- **Reason**: Unsafe operations not practical in Razor page context

## 🚀 Performance and Best Practices

### Switch Expressions
- More efficient than traditional switch statements
- Better for functional-style programming
- Exhaustiveness checking at compile time

### Using Declarations
- Reduces nesting and improves readability
- Automatic disposal at end of scope
- Better than traditional using statements for simple cases

### Static Local Functions
- Cannot accidentally capture local variables
- Better performance due to no closure allocation
- Compile-time safety for pure functions

### Asynchronous Streams
- Memory efficient for large data sets
- Supports cancellation tokens
- Ideal for streaming scenarios

## 📊 Feature Summary

| Feature | Razor Support | Notes |
|---------|---------------|-------|
| Switch Expressions | ✅ Full | Works in all contexts |
| Property Patterns | ✅ Full | Complete pattern matching |
| Tuple Patterns | ✅ Full | All tuple operations |
| Positional Patterns | ✅ Full | With custom Deconstruct |
| Using Declarations | ✅ Full | Automatic disposal |
| Static Local Functions | ✅ Full | Better performance |
| Indices and Ranges | ✅ Full | All syntax variations |
| Null-Coalescing Assignment | ✅ Full | ??= operator |
| Asynchronous Streams | ✅ Full | IAsyncEnumerable support |
| Enhanced Interpolated Strings | ✅ Full | Both syntaxes |
| Readonly Members | ⚠️ Limited | Code-behind only |
| Default Interface Methods | ⚠️ Limited | Separate files |
| Nullable Reference Types | ⚠️ Limited | Project-wide setting |
| Disposable Ref Structs | ❌ No | Unsafe context |
| Unmanaged Constructed Types | ❌ No | Unsafe context |
| Stackalloc in Expressions | ❌ No | Unsafe context |

## 🔧 Build Requirements

- .NET Core 3.0+ (for full C# 8.0 support)
- Nullable reference types enabled in project file
- No additional packages required for basic features

## 🎯 Testing Recommendations

1. Build the project to see nullable reference type warnings
2. Test async streams with different data sizes
3. Verify switch expression exhaustiveness checking
4. Test pattern matching with various input combinations
5. Validate using declaration disposal behavior
