using System.CommandLine;
using ScaffoldHelpGenerator.Models;
using ScaffoldHelpGenerator.Services;

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
    try
    {
        Console.WriteLine("ScaffoldHelpGenerator - Generating dotnet scaffold documentation");
        Console.WriteLine();

        // Check if output file exists and handle overwrite
        if (output.Exists && !force)
        {
            Console.Write($"Output file '{output.FullName}' already exists. Overwrite? (y/n) ");
            var response = Console.ReadLine()?.Trim().ToLowerInvariant();
            if (response != "y")
            {
                Console.Error.WriteLine("Operation cancelled by user.");
                Environment.Exit(1);
                return;
            }
        }

        // Create output directory if needed
        try
        {
            if (output.Directory != null && !output.Directory.Exists)
            {
                output.Directory.Create();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: Could not create output directory '{output.Directory?.FullName}'. {ex.Message}");
            Environment.Exit(4);
            return;
        }

        var executor = new CommandExecutor();
        var parser = new HelpParser();
        var generator = new DocumentGenerator();

        // Get version information
        var version = await executor.GetVersionAsync();

        // Verify dotnet scaffold is available by getting root help
        Console.WriteLine("Collecting help documentation...");
        string rootHelp;
        try
        {
            rootHelp = await executor.GetHelpAsync("");
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("not found"))
        {
            Console.Error.WriteLine(ex.Message);
            Environment.Exit(2);
            return;
        }
        catch (TimeoutException ex)
        {
            Console.Error.WriteLine(ex.Message);
            Environment.Exit(6);
            return;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: Failed to execute 'dotnet scaffold'. {ex.Message}");
            Environment.Exit(3);
            return;
        }

        // Build command tree starting from root
        var scaffoldRootCommand = new CommandInfo
        {
            FullCommandPath = "",
            Name = "scaffold",
            Description = "dotnet scaffold root command",
            RawHelpOutput = rootHelp
        };

        var allCommands = new List<CommandInfo> { scaffoldRootCommand };

        // Discover and collect all commands recursively
        await CollectCommandsRecursively(scaffoldRootCommand, executor, parser);

        var totalCommands = CountCommands(scaffoldRootCommand);
        Console.WriteLine();
        Console.WriteLine("Generating documentation...");
        Console.WriteLine($"Writing to: {output.FullName}");

        // Generate the markdown document
        var markdown = generator.GenerateFullDocument(allCommands, version);

        // Write to file
        try
        {
            await File.WriteAllTextAsync(output.FullName, markdown);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: Could not write to output file '{output.FullName}'. {ex.Message}");
            Environment.Exit(5);
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"Done! Generated documentation for {totalCommands} commands.");
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        Environment.Exit(99);
    }
}, outputOption, forceOption);

return await rootCommand.InvokeAsync(args);

async Task CollectCommandsRecursively(CommandInfo parentCommand, CommandExecutor executor, HelpParser parser)
{
    var subCommands = parser.ExtractSubCommands(parentCommand.RawHelpOutput);
    
    if (subCommands.Count == 0)
    {
        return;
    }

    var commandIndex = 0;
    var totalSubCommands = subCommands.Count;
    
    var parentPath = string.IsNullOrEmpty(parentCommand.FullCommandPath) ? "" : $"{parentCommand.FullCommandPath} ";
    var displayName = string.IsNullOrEmpty(parentCommand.FullCommandPath) ? "root" : parentCommand.Name;
    
    Console.WriteLine($"  → Found {totalSubCommands} sub-commands under {displayName}");

    foreach (var (name, description) in subCommands)
    {
        commandIndex++;
        var fullPath = $"{parentPath}{name}".Trim();
        
        Console.Write($"  → [{commandIndex}/{totalSubCommands}] Processing {fullPath}... ");
        
        try
        {
            var help = await executor.GetHelpAsync(fullPath);
            
            var commandInfo = new CommandInfo
            {
                FullCommandPath = fullPath,
                Name = name,
                Description = description,
                RawHelpOutput = help
            };
            
            parentCommand.SubCommands.Add(commandInfo);
            Console.WriteLine("done");
            
            // Recursively collect sub-commands
            await CollectCommandsRecursively(commandInfo, executor, parser);
        }
        catch (TimeoutException ex)
        {
            Console.Error.WriteLine();
            Console.Error.WriteLine(ex.Message);
            Environment.Exit(6);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine();
            Console.Error.WriteLine($"Error processing '{fullPath}': {ex.Message}");
            Environment.Exit(3);
        }
    }
}

int CountCommands(CommandInfo command)
{
    var count = 1; // Count this command
    foreach (var subCommand in command.SubCommands)
    {
        count += CountCommands(subCommand);
    }
    return count;
}
