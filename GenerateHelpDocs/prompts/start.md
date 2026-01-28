I want you to help me create an instructions file that I can give to Copilot to create a new .NET app. The app should be a .NET console app that uses the latest version of System.CommandLine for all the cli parameters and commands.
The app will generate a text file that contains the help output from `dotnet scaffold`. This is for a new version of `dotnet scaffold` which hasn't been documented on the internet.
Below I have pasted the output of `dotnet scaffold aspnet --help` and `dotnet scaffold aspire --help`. Each of these command has options and commands. For the sub-commands you can get the help output. For example `dotnet scaffold aspnet razorpages-crud --help`.
The app should call `dotnet scaffold aspnet --help` and `dotnet scaffold aspire --help`. The app should use the help output to figure out all the sub-commands which have help and then get the help output for all sub-commands.
This content should be written to a single file.
The goal of this file will be to provide context to copilot on how to call `dotnet scaffold`.

Help me create the instructions file, write it to the `prompts` folder and name the file generate-docs.txt. Ask any clarifying questions. The generated instructions file should be detailed and high quality so that the full application can be generated.

### `dotnet scaffold aspnet --help`

```
dotnet scaffold aspnet --help
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
dotnet scaffold aspire --help
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