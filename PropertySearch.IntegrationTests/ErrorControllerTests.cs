using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using PropertySearch.IntegrationTests.Helpers;
using PropertySearchApp;
using System.Net;

namespace PropertySearch.IntegrationTests;

[Collection(name: "web-application-factory")]
public class ErrorControllerTests
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Startup> _factory;
    public ErrorControllerTests(CustomWebApplicationFactory<Startup> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true
        });
    }

    [Fact]
    public async Task NotFoundTest_ShouldRetunNotFoundPage_WhenUrlDoesNotExist()
    {
        // Arrange
        string url = "/wrong-url";

        //Act
        var defaultPage = await _client.GetAsync(url);

        // Assert
        defaultPage.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
