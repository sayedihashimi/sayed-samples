# ScaffoldHelpGenerator

A .NET 8 console application that generates comprehensive help documentation for `dotnet scaffold` commands.

## Overview

This tool recursively discovers all `dotnet scaffold` commands and generates a single Markdown file containing complete help documentation. The output is optimized for consumption by GitHub Copilot to provide context about how to use `dotnet scaffold`.

## Prerequisites

- .NET 8 SDK
- `dotnet scaffold` tool installed globally:
  ```bash
  dotnet tool install -g Microsoft.dotnet-scaffold
  ```

## Building

```bash
cd GenerateHelpDocs/src/ScaffoldHelpGenerator
dotnet build
```

## Usage

### Basic Usage

Generate documentation with default settings (outputs to `full-help.md` in current directory):

```bash
dotnet run
```

### Specify Output File

```bash
dotnet run -- -o scaffold-docs.md
dotnet run -- --output C:\docs\scaffold-help.md
```

### Force Overwrite

Skip the overwrite prompt for existing files:

```bash
dotnet run -- -o output.md --force
dotnet run -- -o output.md -f
```

### Show Help

```bash
dotnet run -- --help
```

## Command-Line Options

| Option | Short | Description | Default |
|--------|-------|-------------|---------|
| `--output` | `-o` | Path to the output file | `full-help.md` in current directory |
| `--force` | `-f` | Overwrite output file without prompting | `false` |

## How It Works

1. **Discovery**: Starts with `dotnet scaffold --help` to discover all top-level commands
2. **Recursion**: For each discovered command, recursively fetches help and discovers sub-commands
3. **Collection**: Collects all help output in a hierarchical structure
4. **Generation**: Generates a Markdown file with:
   - Metadata (timestamp, version, purpose)
   - Table of contents with anchor links
   - Complete help output for each command
5. **Output**: Writes the formatted documentation to the specified file

## Output Format

The generated Markdown file includes:

- Header with generation metadata
- Hierarchical table of contents
- Complete help documentation for each command
- Proper formatting with code blocks and sections

Example structure:
```markdown
# dotnet scaffold Command Reference

> **Generated**: 2026-01-28T04:15:00Z
> **Generator**: ScaffoldHelpGenerator v1.0.0
> **dotnet scaffold version**: 1.0.0
> **Purpose**: Complete help documentation for dotnet scaffold...

## Table of Contents
- [scaffold](#scaffold)
  - [aspnet](#aspnet)
    - [blazor-empty](#aspnet-blazor-empty)
    ...

## scaffold
### Command
```
dotnet scaffold
```
### Help Output
```
[Full help output here]
```
...
```

## Error Handling

The application provides clear error messages for common scenarios:

- **dotnet scaffold not found** (exit code 2)
- **Command execution failed** (exit code 3)
- **Output directory creation failed** (exit code 4)
- **Output file write failed** (exit code 5)
- **Command timeout** (exit code 6)

All errors are written to stderr for proper error handling in scripts.

## Architecture

```
ScaffoldHelpGenerator/
├── Models/
│   └── CommandInfo.cs          # Command data model
├── Services/
│   ├── CommandExecutor.cs      # Execute dotnet scaffold commands
│   ├── HelpParser.cs          # Parse help output
│   └── DocumentGenerator.cs    # Generate markdown output
└── Program.cs                  # Entry point with CLI setup
```

## License

This project is part of the sayed-samples repository.
