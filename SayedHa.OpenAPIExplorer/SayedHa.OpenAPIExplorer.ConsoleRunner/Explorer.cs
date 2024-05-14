using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace SayedHa.OpenAPIExplorer.ConsoleRunner; 
public class Explorer {
	public Explorer(string openApiSpecFilepath) {
		Document = new OpenApiStreamReader().Read(File.OpenRead(openApiSpecFilepath), out var diagnostic);
	}

	internal OpenApiDocument? Document { get; private set; }

	public List<DocPathWithOperation> GetEndpointsWithOperation() {
		if (Document is null) {
			throw new ArgumentException("Document is null");
		}

		var result = new List<DocPathWithOperation>();

		foreach(var path in Document.Paths) {
			foreach(var op in path.Value.Operations) {
				result.Add(new DocPathWithOperation(op.Key, path.Key));
			}
		}

		return result;
	}
	public EndpointWithInfo GetEndpointFor(OperationType opType, string path) {
		if (Document is null) {
			throw new ArgumentException("Document is null");
		}
		return new EndpointWithInfo(Document, opType, path);
	}
}
public class DocPathWithOperation {
	public DocPathWithOperation(OperationType operationType, string path) {
		OperationType = operationType;
		Path = path;
	}
	public OperationType OperationType { get; set; }
	public string Path { get; set; }

	override public string ToString() {
		return $"{OperationType.ToString().PadLeft(8)} {Path}";
	}
}
public class EndpointWithInfo {
	public EndpointWithInfo(OpenApiDocument document,OperationType opType, string path) {
		Debug.Assert(document != null);
		Debug.Assert(!string.IsNullOrWhiteSpace(path));

		OperationType = opType;
		Path = path;
		var foundPath = document.Paths.FirstOrDefault(p => p.Key == path);
		if (foundPath.Value is null) {
			throw new ArgumentException($"Path {path} not found");
		}
		var foundOp = foundPath.Value.Operations.FirstOrDefault(o => o.Key == opType);
		if (foundOp.Value is null) {
			throw new ArgumentException($"Operation {opType} not found for path {path}");
		}

		var security = foundOp.Value.Security;
		foreach (var sec in security) {
			Security.Add(sec);
		}

		foreach(var param in foundOp.Value.Parameters) {
			Parameters.Add(param);
		}
		foreach (var response in foundOp.Value.Responses) {
			ResponsesWithKey.Add((response.Key, response.Value));
		}

		Summary = foundOp.Value.Summary;
	}
	public string Path { get; set; }
	public string Description { get; set; }
	public string Summary { get; set; }
	public OperationType OperationType { get; set; }
	public List<OpenApiSecurityRequirement> Security { get; set; } = new List<OpenApiSecurityRequirement>();
	public List<OpenApiParameter> Parameters { get; set; } = new List<OpenApiParameter>();
	// public List<OpenApiResponse> Response { get; set; } = new List<OpenApiResponse>();
	public List<(string key,OpenApiResponse response)>ResponsesWithKey = new List<(string key, OpenApiResponse response)>();
}