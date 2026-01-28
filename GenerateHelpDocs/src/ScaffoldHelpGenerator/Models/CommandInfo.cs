namespace ScaffoldHelpGenerator.Models;

public class CommandInfo
{
    public string FullCommandPath { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string RawHelpOutput { get; set; } = "";
    public List<CommandInfo> SubCommands { get; set; } = new();
}
