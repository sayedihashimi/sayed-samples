namespace ApiApplication2;

using Boxed.AspNetCore.Swagger;
using Boxed.AspNetCore.Swagger.OperationFilters;
using Boxed.AspNetCore.Swagger.SchemaFilters;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        options.DescribeAllParametersInCamelCase();
        options.EnableAnnotations();

        // Add the XML comment file for this assembly, so its contents can be displayed.
        options.IncludeXmlCommentsIfExists(typeof(Startup).Assembly);

        options.OperationFilter<ClaimsOperationFilter>();
        options.OperationFilter<ForbiddenResponseOperationFilter>();
        options.OperationFilter<ProblemDetailsOperationFilter>();
        options.OperationFilter<UnauthorizedResponseOperationFilter>();

        // Show a default and example model for JsonPatchDocument<T>.
        options.SchemaFilter<JsonPatchDocumentSchemaFilter>();

        var info = new OpenApiInfo()
        {
            Title = AssemblyInformation.Current.Product,
            Description = AssemblyInformation.Current.Description,
            Version = "v1",
        };
        options.SwaggerDoc("v1", info);
    }
}
