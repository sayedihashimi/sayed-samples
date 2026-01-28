Help me create a detailed instructions file that I can give to Copilot to create a new .NET app. Write it to the `prompts` folder and name the file `generate-docs.md`. The generated instructions file should be detailed and high quality so that the full application can be generated in one go.

## Application Requirements

### Project Details
- **App Name**: `ScaffoldHelpGenerator`
- **Framework**: .NET 10 console application
- **CLI Library**: Latest version of `System.CommandLine` for all CLI parameters and commands
- **Entry Point**: Use top-level statements

### Command-Line Interface
The app should have these options:
- `--output` / `-o`: Path to the output file. Default: `full-help.md` in the current working directory
- `--force` / `-f`: Overwrite output file without prompting. Default: `false`

If the output file already exists and `--force` is not specified, prompt the user to confirm overwrite (y/n). If they decline, exit with a clear message and exit code 1.

### Core Functionality
The app generates comprehensive help documentation for `dotnet scaffold` by:

1. **Starting from the root**: Call `dotnet scaffold --help` to discover all top-level commands (aspnet, aspire, and any others that may exist)
2. **Recursive discovery**: For each command that has sub-commands, recursively get their help output
3. **Full documentation**: Continue recursing until all commands at every level are documented

This approach ensures the app automatically discovers any new commands added to `dotnet scaffold` without code changes.

### Output Format
Generate a **Markdown file** (optimized for Copilot consumption) with:
- **Metadata header**: Generation timestamp (ISO 8601 UTC), generator version, `dotnet scaffold` version (if available), and a purpose statement explaining the file is for Copilot context
- **Table of Contents**: Dynamically generated with anchors to each command section
- **Command sections**: For each discovered command, include:
  - The full command to invoke it (e.g., `dotnet scaffold aspnet blazor-empty`)
  - The complete verbatim help output in a fenced code block

### Error Handling
The app should fail with clear error messages and specific exit codes for:
- `dotnet scaffold` not installed (suggest installation command)
- Command execution failures (include stderr output)
- Output directory creation failures
- Output file write failures  
- Command timeout (use 30 second timeout)

All errors should be written to stderr.

### Progress Indication
Show console progress updates as commands are processed, including:
- Which command group is being processed
- How many sub-commands were found
- Status for each sub-command being processed
- Final summary of total commands documented

### Architecture
Suggest a clean class structure with:
- `CommandExecutor` for running processes
- `HelpParser` for extracting sub-commands from help output
- `DocumentGenerator` for creating the markdown
- `CommandInfo` model for storing command data

## Sample Help Output

Below are examples of the help output format. The app should parse the "Commands:" section to discover sub-commands.

### `dotnet scaffold aspnet --help`

```
Description:
  Commands related to ASP.NET project scaffolding

Usage:
  dotnet-scaffold aspnet [command] [options]

Options:
  -?, -h, --help  Show help and usage information

Commands:
  blazor-empty        Add an empty razor component to a given project
  razorview-empty     Add an empty razor view to a given project
  razorpage-empty     Add an empty razor page to a given project
  apicontroller       Add an empty API Controller to a given project
  mvccontroller       Add an empty MVC Controller to a given project
  apicontroller-crud  Create an API controller with REST actions to create, read, update, delete, and list entities
  mvccontroller-crud  Create a MVC controller with read/write actions and views using Entity Framework
  blazor-crud         Generates Razor Components using Entity Framework for Create, Delete, Details, Edit and List operations for the given model
  razorpages-crud     Generates Razor pages using Entity Framework for Create, Delete, Details, Edit and List operations for the given model
  views               Generates Razor views for Create, Delete, Details, Edit and List operations for the given model
  minimalapi          Generates an endpoints file (with CRUD API endpoints) given a model and optional DbContext.
  area                Creates a MVC Area folder structure.
  blazor-identity     Add blazor identity to a project.
  identity            Add ASP.NET Core identity to a project.
  entra-id            Add Entra auth
```

### `dotnet scaffold aspire --help`

```
Description:
  Commands related to Aspire project scaffolding

Usage:
  dotnet-scaffold aspire [command] [options]

Options:
  -?, -h, --help  Show help and usage information

Commands:
  caching   Modify Aspire project to make it caching ready.
  database  Modify Aspire project to make it database ready.
  storage   Modify Aspire project to make it storage ready.
```