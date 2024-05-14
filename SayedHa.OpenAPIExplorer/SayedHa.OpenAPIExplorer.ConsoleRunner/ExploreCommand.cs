using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Spectre.Console;

namespace SayedHa.OpenAPIExplorer.ConsoleRunner; 
public class ExploreCommand : CommandBase {
    private IReporter _reporter;
    public ExploreCommand(IReporter reporter) {
        _reporter = reporter;
    }
    public override Command CreateCommand() =>
        new Command(name: "explore", description: "Explorer an OpenAPI spec") {
            CommandHandler.Create<string,bool>(async (openApiFilePath,verbose) => {
                _reporter.EnableVerbose = verbose;

				var explorer = new Explorer(openApiFilePath);
				var endpoints = explorer.GetEndpointsWithOperation();

                PrintoutEndpoints(endpoints);

                var endpoint = PromptForEndpoint(endpoints);

                var endpointWithInfo = explorer.GetEndpointFor(endpoint.OperationType, endpoint.Path);
                PrintEndpointWithInfo(endpointWithInfo);
                // added here to avoid async/await warning
                await Task.Delay(1000);
            }),
            ArgumentOpenApiFilePath(),
            OptionVerbose(),
        };
    protected Option OptionPackages() =>
        new Option(new string[] { "--paramname" }, "TODO: update param description") {
            Argument = new Argument<string>(name: "paramname")
        };

    protected Argument ArgumentOpenApiFilePath() =>
        new Argument<string>(
            name: "openApiFilePath",
				description: "The path to the OpenAPI file to explore"
			);

    protected void PrintoutEndpoints(List<DocPathWithOperation> endpoints) {
		_reporter.WriteLine("Endpoints:");
		foreach (var item in endpoints) {
            // var opString = new 
            _reporter.WriteLine($"{item.OperationType.ToString().PadLeft(8)} {item.Path}");
		}
	}
    protected DocPathWithOperation PromptForEndpoint(List<DocPathWithOperation> endpoints) =>
		AnsiConsole.Prompt(
			new SelectionPrompt<DocPathWithOperation>()
				.Title("Select endpoint")
				.AddChoices(endpoints)
		);
    protected void ExplorePath(DocPathWithOperation pathAndOperation) {
        
    }
    protected void PrintEndpointWithInfo(EndpointWithInfo endpoint) {
        // get /api/Contact - Gets all contacts
        //     Security: OAuth2
        //         Scopes: 
        //             read:contacts
        //             write:contacts

        var sb = new StringBuilder();
        sb.AppendLine($"{endpoint.OperationType.ToString().PadLeft(8)}");
        sb.AppendLine($" {endpoint.Path}");
        if (!string.IsNullOrWhiteSpace(endpoint.Summary)) {
            sb.AppendLine($" - {endpoint.Summary}");
        }
        if(endpoint.Security.Count > 0) {
			sb.Append("    Security: ");
			foreach (var sec in endpoint.Security) {
				foreach (var secItem in sec) {
					sb.AppendLine($" {secItem.Key.Type}");
					if (secItem.Value.Any()) {
						sb.AppendLine("      Scopes");
					}
					foreach (var scope in secItem.Value) {
						sb.AppendLine($"          {scope}");
					}
				}
			}
		}


		_reporter.WriteLine(sb.ToString());
	}
}
