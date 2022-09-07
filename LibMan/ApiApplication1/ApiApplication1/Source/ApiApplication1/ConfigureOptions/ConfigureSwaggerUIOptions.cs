namespace ApiApplication1.ConfigureOptions;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

public class ConfigureSwaggerUIOptions : IConfigureOptions<SwaggerUIOptions>
{
    private readonly IApiVersionDescriptionProvider apiVersionDescriptionProvider;

    public ConfigureSwaggerUIOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider) =>
        this.apiVersionDescriptionProvider = apiVersionDescriptionProvider;

    public void Configure(SwaggerUIOptions options)
    {
        // Set the Swagger UI browser document title.
        options.DocumentTitle = AssemblyInformation.Current.Product;
        // Set the Swagger UI to render at '/'.
        options.RoutePrefix = string.Empty;

        options.DisplayOperationId();
        options.DisplayRequestDuration();

        foreach (var apiVersionDescription in this.apiVersionDescriptionProvider
            .ApiVersionDescriptions
            .OrderByDescending(x => x.ApiVersion))
        {
            options.SwaggerEndpoint(
                $"/swagger/{apiVersionDescription.GroupName}/swagger.json",
                $"Version {apiVersionDescription.ApiVersion}");
        }
    }
}
