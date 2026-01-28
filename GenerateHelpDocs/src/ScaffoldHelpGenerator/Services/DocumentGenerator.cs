using System.Text;
using ScaffoldHelpGenerator.Models;

namespace ScaffoldHelpGenerator.Services;

public class DocumentGenerator
{
    public string GenerateFullDocument(List<CommandInfo> commands, string? version)
    {
        var sb = new StringBuilder();
        
        // Header with metadata
        sb.AppendLine("# dotnet scaffold Command Reference");
        sb.AppendLine();
        sb.AppendLine($"> **Generated**: {DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}");
        sb.AppendLine("> **Generator**: ScaffoldHelpGenerator v1.0.0");
        sb.AppendLine($"> **dotnet scaffold version**: {version ?? "unknown"}");
        sb.AppendLine("> **Purpose**: This document provides complete help documentation for the `dotnet scaffold` command and all its sub-commands. Use this as a reference for understanding available commands, options, and usage patterns.");
        sb.AppendLine();
        sb.AppendLine("---");
        sb.AppendLine();
        
        // Table of Contents
        sb.AppendLine("## Table of Contents");
        sb.AppendLine();
        sb.Append(GenerateTableOfContents(commands));
        sb.AppendLine();
        sb.AppendLine("---");
        sb.AppendLine();
        
        // Command sections
        foreach (var command in commands)
        {
            sb.Append(GenerateCommandSections(command));
        }

        return sb.ToString();
    }

    public string GenerateTableOfContents(List<CommandInfo> commands)
    {
        var sb = new StringBuilder();
        
        foreach (var command in commands)
        {
            AppendTocEntry(sb, command, 0);
        }
        
        return sb.ToString();
    }

    private void AppendTocEntry(StringBuilder sb, CommandInfo command, int level)
    {
        var indent = new string(' ', level * 2);
        var anchor = CreateAnchor(command.FullCommandPath);
        var displayName = string.IsNullOrEmpty(command.FullCommandPath) ? "scaffold" : command.Name;
        
        sb.AppendLine($"{indent}- [{displayName}](#{anchor})");
        
        foreach (var subCommand in command.SubCommands)
        {
            AppendTocEntry(sb, subCommand, level + 1);
        }
    }

    private string GenerateCommandSections(CommandInfo command)
    {
        var sb = new StringBuilder();
        
        sb.Append(GenerateCommandSection(command));
        
        foreach (var subCommand in command.SubCommands)
        {
            sb.Append(GenerateCommandSections(subCommand));
        }
        
        return sb.ToString();
    }

    public string GenerateCommandSection(CommandInfo command)
    {
        var sb = new StringBuilder();
        
        var displayName = string.IsNullOrEmpty(command.FullCommandPath) 
            ? "scaffold" 
            : command.FullCommandPath.Replace(" ", " ");
        
        sb.AppendLine($"## {displayName}");
        sb.AppendLine();
        sb.AppendLine("### Command");
        sb.AppendLine("```");
        
        if (string.IsNullOrEmpty(command.FullCommandPath))
        {
            sb.AppendLine("dotnet scaffold");
        }
        else
        {
            sb.AppendLine($"dotnet scaffold {command.FullCommandPath}");
        }
        
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("### Help Output");
        sb.AppendLine("```");
        sb.AppendLine(command.RawHelpOutput);
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("---");
        sb.AppendLine();
        
        return sb.ToString();
    }

    private string CreateAnchor(string commandPath)
    {
        if (string.IsNullOrEmpty(commandPath))
        {
            return "scaffold";
        }
        
        return commandPath.Replace(" ", "-").ToLowerInvariant();
    }
}
