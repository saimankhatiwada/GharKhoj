using System.Net;
using System.Net.Http.Json;
using GharKhoj.Api.Models.Properties;
using GharKhoj.IntegrationTests.Infrastructure;
using Org.BouncyCastle.Ocsp;
using Xunit;

namespace GharKhoj.IntegrationTests.Tests;

public sealed class PropertyTests(GharKhojWebAppFactory factory) : IntegrationTestFixture(factory)
{
    [Fact]
    public async Task CreateProperty_ShouldSucceed_WithValidParameters()
    {
        PropertyDto propertyDto = TestsData.Properties.CreateMockProperty();
        HttpClient client = await CreateAuthorizedClientAsync();

        HttpResponseMessage response = await client.PostAsJsonAsync(Routes.Properties.Create, propertyDto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        Assert.NotNull(await response.Content.ReadFromJsonAsync<string>());
    }
}
