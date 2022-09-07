namespace ApiApplication3;

using ApiApplication3.OperationFilters;
using Boxed.AspNetCore.Swagger;
using Boxed.AspNetCore.Swagger.OperationFilters;
using Boxed.AspNetCore.Swagger.SchemaFilters;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider provider;

    public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider provider) =>
        this.provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        options.DescribeAllParametersInCamelCase();
        options.EnableAnnotations();

        // Add the XML comment file for this assembly, so its contents can be displayed.
        options.IncludeXmlCommentsIfExists(typeof(Startup).Assembly);

        options.OperationFilter<ApiVersionOperationFilter>();
        options.OperationFilter<ClaimsOperationFilter>();
        options.OperationFilter<ForbiddenResponseOperationFilter>();
        options.OperationFilter<ProblemDetailsOperationFilter>();
        options.OperationFilter<UnauthorizedResponseOperationFilter>();

        // Show a default and example model for JsonPatchDocument<T>.
        options.SchemaFilter<JsonPatchDocumentSchemaFilter>();

        foreach (var apiVersionDescription in this.provider.ApiVersionDescriptions)
        {
            var info = new OpenApiInfo()
            {
                Title = AssemblyInformation.Current.Product,
                Description = apiVersionDescription.IsDeprecated ?
                    $"{AssemblyInformation.Current.Description} This API version has been deprecated." :
                    AssemblyInformation.Current.Description,
                Version = apiVersionDescription.ApiVersion.ToString(),
            };
            options.SwaggerDoc(apiVersionDescription.GroupName, info);
        }
    }
}
