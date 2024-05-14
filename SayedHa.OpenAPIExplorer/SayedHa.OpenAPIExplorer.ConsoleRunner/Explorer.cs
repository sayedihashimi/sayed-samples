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
}
