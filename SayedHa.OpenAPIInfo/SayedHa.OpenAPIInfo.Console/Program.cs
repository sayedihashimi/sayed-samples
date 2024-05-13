// See https://aka.ms/new-console-template for more information

using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Diagnostics;

var appDir = AppDomain.CurrentDomain.BaseDirectory;
var pathToOpenApiJson = Path.Combine(appDir,"assets", "petstore.swagger.json");

Console.WriteLine($"app dir: {appDir}");
Console.WriteLine($"path to openapi json: {pathToOpenApiJson}, exists: {File.Exists(pathToOpenApiJson)}");


var openApiDoc = new OpenApiStreamReader().Read(File.OpenRead(pathToOpenApiJson), out var diagnostic);

PrintOutSecurityForEndpoint(openApiDoc,OperationType.Get, "/pet/findByStatus");
PrintOutEndpointsAndAuth(openApiDoc);

var outputString = openApiDoc.Serialize(OpenApiSpecVersion.OpenApi3_0, OpenApiFormat.Json);

// Console.WriteLine(outputString);

Console.WriteLine("Press enter to exit...");
Console.ReadLine();

void PrintOutSecurityForEndpoint(OpenApiDocument openApiDoc, OperationType opType,string path) {
	Debug.Assert(openApiDoc != null);
	Debug.Assert(!string.IsNullOrEmpty(path));

	var foundPath = openApiDoc.Paths.FirstOrDefault(x => x.Key == path);

	if(foundPath.Value == null) {
		Console.WriteLine($"Path {path} not found in OpenAPI document");
		return;
	}

	var foundOperation = foundPath.Value.Operations.FirstOrDefault(x => x.Key == opType);

	if(foundOperation.Value == null) {
		Console.WriteLine($"Operation {opType} not found in path {path}");
		return;
	}

	var security = foundOperation.Value.Security;
	foreach (var sec in security) {
		foreach(var secItem in sec) {
			Console.WriteLine($"Security: {secItem.Key} - {secItem.Key.Type}");
			
			secItem.Value.ToList().ForEach(x => Console.WriteLine($"    - {x}"));
		}
	}
}
void PrintOutEndpointsAndAuth(OpenApiDocument openApiDoc) {
	Debug.Assert(openApiDoc != null);

	foreach(var path in openApiDoc.Paths) {
		Console.WriteLine($"Path: {path.Key}");
		foreach (var operation in path.Value.Operations) {
			Console.WriteLine($"  {operation.Key}");

			var security = operation.Value.Security;
			foreach(var sec in security) {
				foreach(var secItem in sec) {
					Console.WriteLine($"    Security: {secItem.Key} - {secItem.Key.Type}");
					secItem.Value.ToList().ForEach(x => Console.WriteLine($"      - {x}"));
				}
			}
		}



		Console.WriteLine();
	}

}
