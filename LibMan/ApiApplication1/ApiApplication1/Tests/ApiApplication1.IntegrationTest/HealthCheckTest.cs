namespace ApiApplication1.IntegrationTest.Controllers;

using System.Net;
using Xunit;

public class HealthCheckTest : CustomWebApplicationFactory<Program>
{
    private readonly HttpClient client;

    public HealthCheckTest() =>
        this.client = this.CreateClient();

    [Fact]
    public async Task GetStatus_Default_Returns200OkAsync()
    {
        var response = await this.client.GetAsync(new Uri("/status", UriKind.Relative)).ConfigureAwait(false);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetStatusSelf_Default_Returns200OkAsync()
    {
        var response = await this.client.GetAsync(new Uri("/status/self", UriKind.Relative)).ConfigureAwait(false);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
