using System.CommandLine;
using System.CommandLine.Invocation;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

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
}
