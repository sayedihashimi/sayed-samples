using System.Text.RegularExpressions;

namespace ScaffoldHelpGenerator.Services;

public class HelpParser
{
    public List<(string Name, string Description)> ExtractSubCommands(string helpOutput)
    {
        var commands = new List<(string Name, string Description)>();
        
        if (!HasCommandsSection(helpOutput))
        {
            return commands;
        }

        var lines = helpOutput.Split('\n');
        var inCommandsSection = false;

        foreach (var line in lines)
        {
            if (line.Trim().StartsWith("Commands:", StringComparison.OrdinalIgnoreCase))
            {
                inCommandsSection = true;
                continue;
            }

            if (inCommandsSection)
            {
                // Stop when we hit the next section (like "Options:")
                if (line.Length > 0 && !char.IsWhiteSpace(line[0]))
                {
                    break;
                }

                // Match command lines: "  command-name    Description text"
                var match = Regex.Match(line, @"^\s{2}(\S+)\s{2,}(.+)$");
                if (match.Success)
                {
                    var name = match.Groups[1].Value.Trim();
                    var description = match.Groups[2].Value.Trim();
                    commands.Add((name, description));
                }
            }
        }

        return commands;
    }

    public bool HasCommandsSection(string helpOutput)
    {
        return helpOutput.Contains("Commands:", StringComparison.OrdinalIgnoreCase);
    }
}
