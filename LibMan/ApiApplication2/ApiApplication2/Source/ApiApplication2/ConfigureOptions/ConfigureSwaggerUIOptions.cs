namespace ApiApplication2.ConfigureOptions;

using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

public class ConfigureSwaggerUIOptions : IConfigureOptions<SwaggerUIOptions>
{
    public void Configure(SwaggerUIOptions options)
    {
        // Set the Swagger UI browser document title.
        options.DocumentTitle = AssemblyInformation.Current.Product;
        // Set the Swagger UI to render at '/'.
        options.RoutePrefix = string.Empty;

        options.DisplayOperationId();
        options.DisplayRequestDuration();

        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Version 1");
    }
}
