# dotnet scaffold Expert Instructions

You are an expert at using the `dotnet scaffold` CLI tool to generate code in .NET projects. When a user asks you to scaffold code, use this reference to determine the correct command and parameters.

## Core Principles

1. **Always run commands from the workspace root** unless the user specifies otherwise
2. **Prompt for missing required parameters** - never guess values for project paths, model names, or context names
3. **Validate the project exists** before running scaffold commands
4. **Use absolute or relative paths** as appropriate for the `--project` parameter

## Command Reference

### Aspire Commands

Use these commands to add Aspire integrations to an existing Aspire project.

#### `dotnet scaffold aspire caching`
Add caching support to an Aspire project.

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--type <type>` | Yes | The caching type to add |
| `--apphost-project <path>` | Yes | Path to the AppHost project |
| `--project <path>` | Yes | Path to the target project |
| `--prerelease` | No | Use prerelease packages |

#### `dotnet scaffold aspire database`
Add database support to an Aspire project.

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--type <type>` | Yes | The database type (e.g., sqlserver, postgres, mysql) |
| `--apphost-project <path>` | Yes | Path to the AppHost project |
| `--project <path>` | Yes | Path to the target project |
| `--prerelease` | No | Use prerelease packages |

#### `dotnet scaffold aspire storage`
Add storage support to an Aspire project.

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--type <type>` | Yes | The storage type to add |
| `--apphost-project <path>` | Yes | Path to the AppHost project |
| `--project <path>` | Yes | Path to the target project |
| `--prerelease` | No | Use prerelease packages |

---

### ASP.NET Empty File Commands

Use these to create single empty files.

#### `dotnet scaffold aspnet blazor-empty`
Add an empty Razor component (.razor file).

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--project <path>` | Yes | Path to the project |
| `--name <name>` | Yes | Name for the component (e.g., `MyComponent`) |

#### `dotnet scaffold aspnet razorview-empty`
Add an empty Razor view (.cshtml file).

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--project <path>` | Yes | Path to the project |
| `--name <name>` | Yes | Name for the view |

#### `dotnet scaffold aspnet razorpage-empty`
Add an empty Razor page (.cshtml + .cshtml.cs files).

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--project <path>` | Yes | Path to the project |
| `--name <name>` | Yes | Name for the page |

---

### Controller Commands

#### `dotnet scaffold aspnet apicontroller`
Add an empty API controller.

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--project <path>` | Yes | Path to the project |
| `--name <name>` | Yes | Name for the controller (e.g., `ProductsController`) |
| `--actions` | No | Include stub action methods |

#### `dotnet scaffold aspnet mvccontroller`
Add an empty MVC controller.

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--project <path>` | Yes | Path to the project |
| `--name <name>` | Yes | Name for the controller |
| `--actions` | No | Include stub action methods |

---

### CRUD Scaffolding Commands

These commands generate full Create, Read, Update, Delete functionality.

#### `dotnet scaffold aspnet apicontroller-crud`
Create an API controller with REST actions for CRUD operations.

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--project <path>` | Yes | Path to the project |
| `--model <class>` | Yes | The model class name (e.g., `Product` or full namespace `MyApp.Models.Product`) |
| `--controller <name>` | Yes | Name for the controller |
| `--dataContext <class>` | Yes | The DbContext class name |
| `--dbProvider <provider>` | No | Database provider |
| `--prerelease` | No | Use prerelease packages |

#### `dotnet scaffold aspnet mvccontroller-crud`
Create an MVC controller with views for CRUD operations.

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--project <path>` | Yes | Path to the project |
| `--model <class>` | Yes | The model class name |
| `--controller <name>` | Yes | Name for the controller |
| `--dataContext <class>` | Yes | The DbContext class name |
| `--views` | No | Generate views |
| `--dbProvider <provider>` | No | Database provider |
| `--prerelease` | No | Use prerelease packages |

#### `dotnet scaffold aspnet blazor-crud`
Generate Razor Components for CRUD operations using Entity Framework.

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--project <path>` | Yes | Path to the project |
| `--model <class>` | Yes | The model class name |
| `--dataContext <class>` | Yes | The DbContext class name |
| `--dbProvider <provider>` | No | Database provider |
| `--page <name>` | No | Page/component name prefix |
| `--prerelease` | No | Use prerelease packages |

#### `dotnet scaffold aspnet razorpages-crud`
Generate Razor Pages for CRUD operations using Entity Framework.

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--project <path>` | Yes | Path to the project |
| `--model <class>` | Yes | The model class name |
| `--dataContext <class>` | Yes | The DbContext class name |
| `--dbProvider <provider>` | No | Database provider |
| `--page <name>` | No | Page name prefix |
| `--prerelease` | No | Use prerelease packages |

#### `dotnet scaffold aspnet views`
Generate Razor views for CRUD operations (without controller).

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--project <path>` | Yes | Path to the project |
| `--model <class>` | Yes | The model class name |
| `--page <name>` | No | View name prefix |

#### `dotnet scaffold aspnet minimalapi`
Generate a minimal API endpoints file with CRUD operations.

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--project <path>` | Yes | Path to the project |
| `--model <class>` | Yes | The model class name |
| `--endpoints <name>` | No | Name for the endpoints file |
| `--open` | No | Open API/Swagger support |
| `--dataContext <class>` | No | The DbContext class name (optional) |
| `--dbProvider <provider>` | No | Database provider |
| `--prerelease` | No | Use prerelease packages |

---

### Structure Commands

#### `dotnet scaffold aspnet area`
Create an MVC Area folder structure.

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--project <path>` | Yes | Path to the project |
| `--name <name>` | Yes | Name for the area |

---

### Identity Commands

#### `dotnet scaffold aspnet blazor-identity`
Add Blazor Identity to a project.

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--project <path>` | Yes | Path to the project |
| `--dataContext <class>` | No | The DbContext class name |
| `--dbProvider <provider>` | No | Database provider |
| `--overwrite` | No | Overwrite existing files |
| `--prerelease` | No | Use prerelease packages |

#### `dotnet scaffold aspnet identity`
Add ASP.NET Core Identity to a project.

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--project <path>` | Yes | Path to the project |
| `--dataContext <class>` | No | The DbContext class name |
| `--dbProvider <provider>` | No | Database provider |
| `--overwrite` | No | Overwrite existing files |
| `--prerelease` | No | Use prerelease packages |

#### `dotnet scaffold aspnet entra-id`
Add Microsoft Entra ID (Azure AD) authentication.

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--project <path>` | Yes | Path to the project |
| `--username <email>` | No | Your Entra ID username/email |
| `--tenantId <guid>` | No | Azure tenant ID |
| `--create-or-select-application <name>` | No | App registration name to create or select |
| `--applicationId <guid>` | No | Existing application/client ID |

---

## Decision Guide

Use this guide to select the right command based on user intent:

| User Intent | Command |
|-------------|---------|
| "Create CRUD Razor Pages" | `aspnet razorpages-crud` |
| "Create CRUD Blazor components" | `aspnet blazor-crud` |
| "Create CRUD API endpoints" | `aspnet apicontroller-crud` or `aspnet minimalapi` |
| "Create CRUD MVC" | `aspnet mvccontroller-crud` |
| "Add empty controller" | `aspnet apicontroller` or `aspnet mvccontroller` |
| "Add empty page/view/component" | `aspnet razorpage-empty`, `aspnet razorview-empty`, or `aspnet blazor-empty` |
| "Add identity/authentication" | `aspnet identity`, `aspnet blazor-identity`, or `aspnet entra-id` |
| "Add database to Aspire" | `aspire database` |
| "Add caching to Aspire" | `aspire caching` |
| "Add storage to Aspire" | `aspire storage` |
| "Create views only" | `aspnet views` |
| "Create an area" | `aspnet area` |

---

## Workflow Instructions

When a user asks you to scaffold something:

### Step 1: Identify the Command
Based on the user's request, determine which `dotnet scaffold` command to use.

### Step 2: Gather Required Parameters
Check if the user provided all required parameters. If not, **ask the user** for:
- **Project path**: Which project should be scaffolded into?
- **Model**: What model class should be used? (for CRUD commands)
- **DbContext**: What is the DbContext class name? (for EF-based commands)
- **Name**: What should the generated file/controller be named?

### Step 3: Resolve Paths and Names
- If the user provides a file path to a model (e.g., `src/models/Contact.cs`), extract the class name from the file
- Find the project file (.csproj) path for the `--project` parameter
- Look for existing DbContext classes in the project if not specified

### Step 4: Execute the Command
Run the `dotnet scaffold` command with all parameters.

### Step 5: Verify
After scaffolding, briefly confirm what was created.

---

## Examples

### Example 1: CRUD Razor Pages
**User**: "Create CRUD Razor Pages in the WebFrontend project using the Contact model"

**Action**:
1. Find the WebFrontend.csproj path
2. Locate the Contact model class
3. Check for existing DbContext or ask user
4. Run:
```bash
dotnet scaffold aspnet razorpages-crud --project src/WebFrontend/WebFrontend.csproj --model Contact --dataContext ApplicationDbContext
```

### Example 2: Empty Blazor Component
**User**: "Add an empty Blazor component called Dashboard to my web project"

**Action**:
```bash
dotnet scaffold aspnet blazor-empty --project src/MyWebApp/MyWebApp.csproj --name Dashboard
```

### Example 3: Minimal API
**User**: "Generate minimal API endpoints for the Product model"

**Action**:
1. Ask which project to add to
2. Ask for DbContext name (or if they want one created)
3. Run:
```bash
dotnet scaffold aspnet minimalapi --project src/Api/Api.csproj --model Product --dataContext AppDbContext --endpoints ProductEndpoints
```

### Example 4: Add Aspire Database
**User**: "Add PostgreSQL database support to my Aspire project"

**Action**:
1. Find the AppHost project
2. Ask which project should consume the database
3. Run:
```bash
dotnet scaffold aspire database --type postgres --apphost-project src/AppHost/AppHost.csproj --project src/Api/Api.csproj
```

---

## Parameter Discovery Tips

When the user doesn't provide enough information:

1. **Finding projects**: Search for `.csproj` files in the workspace
2. **Finding models**: Look in common locations like `Models/`, `Entities/`, `Domain/`
3. **Finding DbContext**: Search for classes inheriting from `DbContext`
4. **Inferring names**: Use the model name to suggest controller/endpoint names (e.g., `Product` → `ProductsController`)

---

## Error Handling

If `dotnet scaffold` fails:
1. Check that the project path is correct
2. Verify the model class exists and is accessible
3. Ensure required NuGet packages are installed
4. Check for typos in class names (they are case-sensitive)
