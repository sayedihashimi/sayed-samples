# ScaffoldHelpGenerator - Instructions for Copilot

## Overview

Create a .NET 10 console application named `ScaffoldHelpGenerator` that generates comprehensive help documentation for `dotnet scaffold` commands. The generated documentation will be consumed by GitHub Copilot to provide context about how to use `dotnet scaffold`.

## Project Setup

1. Create a new .NET 10 console application named `ScaffoldHelpGenerator`
2. Add the latest version of `System.CommandLine` NuGet package for CLI argument parsing
3. Use top-level statements for the entry point
4. Target .NET 10.0

## Command-Line Interface

The app should use `System.CommandLine` to define the following CLI structure:

### Options

| Option | Short | Description | Default |
|--------|-------|-------------|---------|
| `--output` | `-o` | Path to the output file | `full-help.md` in current directory |
| `--force` | `-f` | Overwrite output file without prompting | `false` |

### Examples

```
ScaffoldHelpGenerator
ScaffoldHelpGenerator -o scaffold-docs.md
ScaffoldHelpGenerator --output C:\docs\scaffold-help.md
ScaffoldHelpGenerator -o output.md --force
```

## Core Functionality

### 1. Output File Handling

- Default output file: `full-help.md` in the current working directory
- If the output file already exists and `--force` is not specified:
  - Prompt the user: "Output file '{path}' already exists. Overwrite? (y/n)"
  - If user enters 'y' or 'Y', proceed with overwrite
  - If user enters anything else, exit with message "Operation cancelled by user." and exit code 1
- Create any necessary parent directories for the output path

### 2. Help Discovery and Collection

The app must recursively discover and collect help documentation for all `dotnet scaffold` commands.

#### Root Command

Start with the root command:
```
dotnet scaffold --help
```

This will return the top-level help which includes all available command groups (e.g., `aspnet`, `aspire`, and any others that may be added in the future). The app should dynamically discover all commands rather than hardcoding specific ones.

#### Parsing Help Output

When parsing the help output from any command, extract:
1. **Description**: The text after "Description:" 
2. **Usage**: The text after "Usage:"
3. **Options**: All options listed under "Options:"
4. **Commands**: All sub-commands listed under "Commands:" section

The Commands section format is:
```
Commands:
  command-name    Description of the command
  another-cmd     Another description
```

Extract command names by parsing lines under the "Commands:" section. Each line has the format:
- Whitespace followed by command name, then whitespace, then description

#### Recursive Discovery

For each discovered sub-command:
1. Run `dotnet scaffold {command-path} --help`
2. Parse the output for any further sub-commands
3. Recursively process any discovered sub-commands
4. Continue until no more sub-commands are found

Example recursion path:
```
dotnet scaffold --help
  → discovers: aspnet, aspire, (and any other top-level commands)
  
dotnet scaffold aspnet --help
  → discovers: blazor-empty, razorview-empty, razorpage-empty, apicontroller, etc.
  
dotnet scaffold aspnet blazor-empty --help
  → may discover further sub-commands (if any)
  → record all options and usage information
  
dotnet scaffold aspire --help
  → discovers: caching, database, storage
  
dotnet scaffold aspire caching --help
  → record all options and usage information
```

This approach ensures the app automatically discovers any new commands added to `dotnet scaffold` without code changes.

### 3. Process Execution

Use `System.Diagnostics.Process` to execute commands:

```csharp
// Configuration for running dotnet scaffold commands
var startInfo = new ProcessStartInfo
{
    FileName = "dotnet",
    Arguments = $"scaffold {commandPath} --help",
    RedirectStandardOutput = true,
    RedirectStandardError = true,
    UseShellExecute = false,
    CreateNoWindow = true
};
```

- Capture both stdout and stderr
- Set a reasonable timeout (30 seconds)
- Handle process execution failures gracefully

### 4. Error Handling

The application should fail with clear error messages for these scenarios:

| Scenario | Error Message | Exit Code |
|----------|---------------|-----------|
| `dotnet scaffold` not installed/found | "Error: 'dotnet scaffold' command not found. Please ensure dotnet-scaffold tool is installed globally using: dotnet tool install -g Microsoft.dotnet-scaffold" | 2 |
| Command execution fails | "Error: Failed to execute '{command}'. Exit code: {code}. Error: {stderr}" | 3 |
| Output directory creation fails | "Error: Could not create output directory '{path}'. {exception message}" | 4 |
| Output file write fails | "Error: Could not write to output file '{path}'. {exception message}" | 5 |
| Timeout waiting for command | "Error: Command '{command}' timed out after 30 seconds" | 6 |

Always write errors to stderr using `Console.Error.WriteLine()`.

## Output File Format

Generate a Markdown file optimized for Copilot consumption. Markdown works well because:
- Copilot understands Markdown structure
- Headers create clear section boundaries
- Code blocks preserve command formatting
- It's human-readable for verification

### Output Structure

```markdown
# dotnet scaffold Command Reference

> **Generated**: {UTC timestamp in ISO 8601 format}
> **Generator**: ScaffoldHelpGenerator v1.0.0
> **dotnet scaffold version**: {output of `dotnet scaffold --version` if available, otherwise "unknown"}
> **Purpose**: This document provides complete help documentation for the `dotnet scaffold` command and all its sub-commands. Use this as a reference for understanding available commands, options, and usage patterns.

---

## Table of Contents

{Dynamically generated based on discovered commands}

- [scaffold](#scaffold)
  - [aspnet](#aspnet)
    - [blazor-empty](#aspnet-blazor-empty)
    - [razorview-empty](#aspnet-razorview-empty)
    - ... (all sub-commands)
  - [aspire](#aspire)
    - [caching](#aspire-caching)
    - ... (all sub-commands)
  - ... (any other top-level commands)

---

## scaffold

### Command
```
dotnet scaffold
```

### Help Output
```
{full verbatim help output from dotnet scaffold --help}
```

---

## aspnet

### Command
```
dotnet scaffold aspnet
```

### Help Output
```
{full verbatim help output from dotnet scaffold aspnet --help}
```

---

## aspnet blazor-empty

### Command
```
dotnet scaffold aspnet blazor-empty
```

### Help Output
```
{full verbatim help output}
```

---

{continue for all discovered commands...}
```

### Formatting Rules

1. Use `##` for top-level commands (aspnet, aspire)
2. Use `##` with full path for sub-commands (aspnet blazor-empty)
3. Create anchor-friendly IDs by replacing spaces with hyphens
4. Preserve the exact help output in fenced code blocks
5. Add horizontal rules (`---`) between major sections
6. Include the exact command needed to invoke each help

## Application Architecture

### Suggested Class Structure

```
ScaffoldHelpGenerator/
├── Program.cs                 # Entry point with System.CommandLine setup
├── Services/
│   ├── CommandExecutor.cs     # Handles running dotnet scaffold commands
│   ├── HelpParser.cs          # Parses help output to extract commands
│   └── DocumentGenerator.cs   # Generates the markdown output
├── Models/
│   ├── CommandInfo.cs         # Represents a command with its help text
│   └── HelpContent.cs         # Parsed help content structure
└── ScaffoldHelpGenerator.csproj
```

### Key Classes

#### CommandInfo Model

```csharp
public class CommandInfo
{
    public string FullCommandPath { get; set; } = "";  // e.g., "aspnet blazor-empty"
    public string Name { get; set; } = "";              // e.g., "blazor-empty"
    public string Description { get; set; } = "";       // Short description from parent
    public string RawHelpOutput { get; set; } = "";     // Full --help output
    public List<CommandInfo> SubCommands { get; set; } = new();
}
```

#### HelpParser

Should provide methods to:
1. `ExtractSubCommands(string helpOutput)` - Returns list of (name, description) tuples
2. `HasCommandsSection(string helpOutput)` - Checks if help has sub-commands

The parser should look for the "Commands:" section and extract command names. Example regex pattern:
```csharp
// Match lines in the Commands section: "  command-name    Description text"
var commandPattern = @"^\s{2}(\S+)\s{2,}(.+)$";
```

#### CommandExecutor

Should provide:
1. `Task<string> GetHelpAsync(string commandPath)` - Runs command and returns output
2. `Task<string?> GetVersionAsync()` - Gets dotnet scaffold version
3. Timeout handling
4. Error detection and reporting

#### DocumentGenerator

Should provide:
1. `string GenerateTableOfContents(CommandInfo root)` - Creates TOC with anchors
2. `string GenerateCommandSection(CommandInfo command)` - Formats single command
3. `string GenerateFullDocument(List<CommandInfo> commands, string version)` - Creates complete doc

## Progress Indication

Since this tool may take time to run all commands, provide console progress updates:

```
ScaffoldHelpGenerator - Generating dotnet scaffold documentation

Collecting help documentation...
  [1/2] Processing aspnet...
    → Found 15 sub-commands
    → Processing aspnet blazor-empty... done
    → Processing aspnet razorview-empty... done
    ... (continue for each)
  [2/2] Processing aspire...
    → Found 3 sub-commands
    → Processing aspire caching... done
    ...

Generating documentation...
Writing to: C:\output\full-help.md

Done! Generated documentation for 20 commands.
```

## Complete Program.cs Example Structure

```csharp
using System.CommandLine;

// Define options
var outputOption = new Option<FileInfo>(
    aliases: new[] { "--output", "-o" },
    description: "Path to the output file",
    getDefaultValue: () => new FileInfo(Path.Combine(Environment.CurrentDirectory, "full-help.md")));

var forceOption = new Option<bool>(
    aliases: new[] { "--force", "-f" },
    description: "Overwrite output file without prompting",
    getDefaultValue: () => false);

// Create root command
var rootCommand = new RootCommand("Generates comprehensive help documentation for dotnet scaffold commands")
{
    outputOption,
    forceOption
};

// Set handler
rootCommand.SetHandler(async (FileInfo output, bool force) =>
{
    // Implementation here:
    // 1. Check if output exists, prompt if needed
    // 2. Verify dotnet scaffold is available
    // 3. Collect all help recursively
    // 4. Generate and write documentation
}, outputOption, forceOption);

return await rootCommand.InvokeAsync(args);
```

## Testing the Application

After building, test with:

```powershell
# Basic run with defaults
dotnet run

# Specify output file
dotnet run -- -o scaffold-reference.md

# Force overwrite
dotnet run -- -o existing-file.md --force

# Show help
dotnet run -- --help
```

## Summary Checklist

- [ ] .NET 10 console app with System.CommandLine (latest version)
- [ ] `--output` / `-o` option with default `full-help.md`
- [ ] `--force` / `-f` option to skip overwrite prompt
- [ ] Prompt before overwriting existing files
- [ ] Start from `dotnet scaffold --help` to discover all top-level commands
- [ ] Recursively discover and document all commands and sub-commands at every level
- [ ] Generate Markdown with table of contents
- [ ] Include metadata header (timestamp, version, purpose)
- [ ] Clear error messages with appropriate exit codes
- [ ] Progress indication during execution
- [ ] Timeout handling for command execution
- [ ] All errors written to stderr
