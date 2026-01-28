using System.Diagnostics;

namespace ScaffoldHelpGenerator.Services;

public class CommandExecutor
{
    private const int TimeoutSeconds = 30;

    public async Task<string> GetHelpAsync(string commandPath)
    {
        var arguments = string.IsNullOrEmpty(commandPath) 
            ? "scaffold --help" 
            : $"scaffold {commandPath} --help";

        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = startInfo };
        
        try
        {
            process.Start();
            
            var outputTask = process.StandardOutput.ReadToEndAsync();
            var errorTask = process.StandardError.ReadToEndAsync();
            
            var timeout = TimeSpan.FromSeconds(TimeoutSeconds);
            if (!process.WaitForExit((int)timeout.TotalMilliseconds))
            {
                process.Kill();
                var command = string.IsNullOrEmpty(commandPath) ? "dotnet scaffold" : $"dotnet scaffold {commandPath}";
                throw new TimeoutException($"Error: Command '{command}' timed out after {TimeoutSeconds} seconds");
            }

            var output = await outputTask;
            var error = await errorTask;

            if (process.ExitCode != 0)
            {
                var command = string.IsNullOrEmpty(commandPath) ? "dotnet scaffold" : $"dotnet scaffold {commandPath}";
                throw new InvalidOperationException($"Error: Failed to execute '{command}'. Exit code: {process.ExitCode}. Error: {error}");
            }

            return output;
        }
        catch (System.ComponentModel.Win32Exception)
        {
            throw new InvalidOperationException("Error: 'dotnet scaffold' command not found. Please ensure dotnet-scaffold tool is installed globally using: dotnet tool install -g Microsoft.dotnet-scaffold");
        }
    }

    public async Task<string?> GetVersionAsync()
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "scaffold --version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = startInfo };
            process.Start();
            
            var outputTask = process.StandardOutput.ReadToEndAsync();
            
            var timeout = TimeSpan.FromSeconds(TimeoutSeconds);
            if (!process.WaitForExit((int)timeout.TotalMilliseconds))
            {
                process.Kill();
                return null;
            }

            if (process.ExitCode != 0)
            {
                return null;
            }

            var output = await outputTask;
            return output.Trim();
        }
        catch
        {
            return null;
        }
    }
}
