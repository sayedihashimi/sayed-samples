using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi;
using System.Diagnostics;

namespace SayedHa.OpenAPIExplorer.ConsoleRunner; 
public class Explorer {
	public Explorer(string openApiSpecFilepath) {
		Document = new OpenApiStreamReader().Read(File.OpenRead(openApiSpecFilepath), out var diagnostic);
	}

	internal OpenApiDocument? Document { get; private set; }

	public List<DocPathWithOperation> GetEndpointsWithOperation() {
		if (Document == null) {
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
}
public class DocPathWithOperation {
	public DocPathWithOperation(OperationType operationType, string path) {
		OperationType = operationType;
		Path = path;
	}
	public OperationType OperationType { get; set; }
	public string Path { get; set; }
}
