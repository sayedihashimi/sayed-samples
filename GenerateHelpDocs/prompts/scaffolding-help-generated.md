# dotnet scaffold Command Reference

> **Generated**: 2026-01-28T04:44:47Z
> **Generator**: ScaffoldHelpGenerator v1.0.0
> **dotnet scaffold version**: 10.0.2
> **Purpose**: This document provides complete help documentation for the `dotnet scaffold` command and all its sub-commands. Use this as a reference for understanding available commands, options, and usage patterns.

---

## Table of Contents

- [dotnet scaffold Command Reference](#dotnet-scaffold-command-reference)
  - [Table of Contents](#table-of-contents)
  - [scaffold](#scaffold)
    - [Command](#command)
    - [Help Output](#help-output)
  - [aspire](#aspire)
    - [Command](#command-1)
    - [Help Output](#help-output-1)
  - [aspire caching](#aspire-caching)
    - [Command](#command-2)
    - [Help Output](#help-output-2)
  - [aspire database](#aspire-database)
    - [Command](#command-3)
    - [Help Output](#help-output-3)
  - [aspire storage](#aspire-storage)
    - [Command](#command-4)
    - [Help Output](#help-output-4)
  - [aspnet](#aspnet)
    - [Command](#command-5)
    - [Help Output](#help-output-5)
  - [aspnet blazor-empty](#aspnet-blazor-empty)
    - [Command](#command-6)
    - [Help Output](#help-output-6)
  - [aspnet razorview-empty](#aspnet-razorview-empty)
    - [Command](#command-7)
    - [Help Output](#help-output-7)
  - [aspnet razorpage-empty](#aspnet-razorpage-empty)
    - [Command](#command-8)
    - [Help Output](#help-output-8)
  - [aspnet apicontroller](#aspnet-apicontroller)
    - [Command](#command-9)
    - [Help Output](#help-output-9)
  - [aspnet mvccontroller](#aspnet-mvccontroller)
    - [Command](#command-10)
    - [Help Output](#help-output-10)
  - [aspnet apicontroller-crud](#aspnet-apicontroller-crud)
    - [Command](#command-11)
    - [Help Output](#help-output-11)
  - [aspnet mvccontroller-crud](#aspnet-mvccontroller-crud)
    - [Command](#command-12)
    - [Help Output](#help-output-12)
  - [aspnet blazor-crud](#aspnet-blazor-crud)
    - [Command](#command-13)
    - [Help Output](#help-output-13)
  - [aspnet razorpages-crud](#aspnet-razorpages-crud)
    - [Command](#command-14)
    - [Help Output](#help-output-14)
  - [aspnet views](#aspnet-views)
    - [Command](#command-15)
    - [Help Output](#help-output-15)
  - [aspnet minimalapi](#aspnet-minimalapi)
    - [Command](#command-16)
    - [Help Output](#help-output-16)
  - [aspnet area](#aspnet-area)
    - [Command](#command-17)
    - [Help Output](#help-output-17)
  - [aspnet blazor-identity](#aspnet-blazor-identity)
    - [Command](#command-18)
    - [Help Output](#help-output-18)
  - [aspnet identity](#aspnet-identity)
    - [Command](#command-19)
    - [Help Output](#help-output-19)
  - [aspnet entra-id](#aspnet-entra-id)
    - [Command](#command-20)
    - [Help Output](#help-output-20)

---

## scaffold

### Command
```
dotnet scaffold
```

### Help Output
```
Description:

Usage:
  dotnet-scaffold [command] [options]

Options:
  -?, -h, --help  Show help and usage information
  --version       Show version information

Commands:
  aspire        Commands related to Aspire project scaffolding
  aspnet        Commands related to ASP.NET project scaffolding
  get-commands


```

---

## aspire

### Command
```
dotnet scaffold aspire
```

### Help Output
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

---

## aspire caching

### Command
```
dotnet scaffold aspire caching
```

### Help Output
```
Description:
  Modify Aspire project to make it caching ready.

Usage:
  dotnet-scaffold aspire caching [options]

Options:
  --type <type>
  --apphost-project <apphost-project>
  --project <project>
  --prerelease
  -?, -h, --help                       Show help and usage information


```

---

## aspire database

### Command
```
dotnet scaffold aspire database
```

### Help Output
```
Description:
  Modify Aspire project to make it database ready.

Usage:
  dotnet-scaffold aspire database [options]

Options:
  --type <type>
  --apphost-project <apphost-project>
  --project <project>
  --prerelease
  -?, -h, --help                       Show help and usage information


```

---

## aspire storage

### Command
```
dotnet scaffold aspire storage
```

### Help Output
```
Description:
  Modify Aspire project to make it storage ready.

Usage:
  dotnet-scaffold aspire storage [options]

Options:
  --type <type>
  --apphost-project <apphost-project>
  --project <project>
  --prerelease
  -?, -h, --help                       Show help and usage information


```

---

## aspnet

### Command
```
dotnet scaffold aspnet
```

### Help Output
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

---

## aspnet blazor-empty

### Command
```
dotnet scaffold aspnet blazor-empty
```

### Help Output
```
Description:
  Add an empty razor component to a given project

Usage:
  dotnet-scaffold aspnet blazor-empty [options]

Options:
  --project <project>
  --name <name>
  -?, -h, --help       Show help and usage information


```

---

## aspnet razorview-empty

### Command
```
dotnet scaffold aspnet razorview-empty
```

### Help Output
```
Description:
  Add an empty razor view to a given project

Usage:
  dotnet-scaffold aspnet razorview-empty [options]

Options:
  --project <project>
  --name <name>
  -?, -h, --help       Show help and usage information


```

---

## aspnet razorpage-empty

### Command
```
dotnet scaffold aspnet razorpage-empty
```

### Help Output
```
Description:
  Add an empty razor page to a given project

Usage:
  dotnet-scaffold aspnet razorpage-empty [options]

Options:
  --project <project>
  --name <name>
  -?, -h, --help       Show help and usage information


```

---

## aspnet apicontroller

### Command
```
dotnet scaffold aspnet apicontroller
```

### Help Output
```
Description:
  Add an empty API Controller to a given project

Usage:
  dotnet-scaffold aspnet apicontroller [options]

Options:
  --project <project>
  --name <name>
  --actions
  -?, -h, --help       Show help and usage information


```

---

## aspnet mvccontroller

### Command
```
dotnet scaffold aspnet mvccontroller
```

### Help Output
```
Description:
  Add an empty MVC Controller to a given project

Usage:
  dotnet-scaffold aspnet mvccontroller [options]

Options:
  --project <project>
  --name <name>
  --actions
  -?, -h, --help       Show help and usage information


```

---

## aspnet apicontroller-crud

### Command
```
dotnet scaffold aspnet apicontroller-crud
```

### Help Output
```
Description:
  Create an API controller with REST actions to create, read, update, delete, and list entities

Usage:
  dotnet-scaffold aspnet apicontroller-crud [options]

Options:
  --project <project>
  --model <model>
  --controller <controller>
  --dataContext <dataContext>
  --dbProvider <dbProvider>
  --prerelease
  -?, -h, --help               Show help and usage information


```

---

## aspnet mvccontroller-crud

### Command
```
dotnet scaffold aspnet mvccontroller-crud
```

### Help Output
```
Description:
  Create a MVC controller with read/write actions and views using Entity Framework

Usage:
  dotnet-scaffold aspnet mvccontroller-crud [options]

Options:
  --project <project>
  --model <model>
  --controller <controller>
  --views
  --dataContext <dataContext>
  --dbProvider <dbProvider>
  --prerelease
  -?, -h, --help               Show help and usage information


```

---

## aspnet blazor-crud

### Command
```
dotnet scaffold aspnet blazor-crud
```

### Help Output
```
Description:
  Generates Razor Components using Entity Framework for Create, Delete, Details, Edit and List operations for the given model

Usage:
  dotnet-scaffold aspnet blazor-crud [options]

Options:
  --project <project>
  --model <model>
  --dataContext <dataContext>
  --dbProvider <dbProvider>
  --page <page>
  --prerelease
  -?, -h, --help               Show help and usage information


```

---

## aspnet razorpages-crud

### Command
```
dotnet scaffold aspnet razorpages-crud
```

### Help Output
```
Description:
  Generates Razor pages using Entity Framework for Create, Delete, Details, Edit and List operations for the given model

Usage:
  dotnet-scaffold aspnet razorpages-crud [options]

Options:
  --project <project>
  --model <model>
  --dataContext <dataContext>
  --dbProvider <dbProvider>
  --page <page>
  --prerelease
  -?, -h, --help               Show help and usage information


```

---

## aspnet views

### Command
```
dotnet scaffold aspnet views
```

### Help Output
```
Description:
  Generates Razor views for Create, Delete, Details, Edit and List operations for the given model

Usage:
  dotnet-scaffold aspnet views [options]

Options:
  --project <project>
  --model <model>
  --page <page>
  -?, -h, --help       Show help and usage information


```

---

## aspnet minimalapi

### Command
```
dotnet scaffold aspnet minimalapi
```

### Help Output
```
Description:
  Generates an endpoints file (with CRUD API endpoints) given a model and optional DbContext.

Usage:
  dotnet-scaffold aspnet minimalapi [options]

Options:
  --project <project>
  --model <model>
  --endpoints <endpoints>
  --open
  --dataContext <dataContext>
  --dbProvider <dbProvider>
  --prerelease
  -?, -h, --help               Show help and usage information


```

---

## aspnet area

### Command
```
dotnet scaffold aspnet area
```

### Help Output
```
Description:
  Creates a MVC Area folder structure.

Usage:
  dotnet-scaffold aspnet area [options]

Options:
  --project <project>
  --name <name>
  -?, -h, --help       Show help and usage information


```

---

## aspnet blazor-identity

### Command
```
dotnet scaffold aspnet blazor-identity
```

### Help Output
```
Description:
  Add blazor identity to a project.

Usage:
  dotnet-scaffold aspnet blazor-identity [options]

Options:
  --project <project>
  --dataContext <dataContext>
  --dbProvider <dbProvider>
  --overwrite
  --prerelease
  -?, -h, --help               Show help and usage information


```

---

## aspnet identity

### Command
```
dotnet scaffold aspnet identity
```

### Help Output
```
Description:
  Add ASP.NET Core identity to a project.

Usage:
  dotnet-scaffold aspnet identity [options]

Options:
  --project <project>
  --dataContext <dataContext>
  --dbProvider <dbProvider>
  --overwrite
  --prerelease
  -?, -h, --help               Show help and usage information


```

---

## aspnet entra-id

### Command
```
dotnet scaffold aspnet entra-id
```

### Help Output
```
Description:
  Add Entra auth

Usage:
  dotnet-scaffold aspnet entra-id [options]

Options:
  --username <username>
  --project <project>
  --tenantId <tenantId>
  --create-or-select-application <create-or-select-application>
  --applicationId <applicationId>
  -?, -h, --help                                                 Show help and usage information


```

---

