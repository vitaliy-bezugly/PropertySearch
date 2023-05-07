using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using PropertySearch.IntegrationTests.Helpers;
using PropertySearchApp;
using System.Net;

namespace PropertySearch.IntegrationTests;

[Collection(name: "web-application-factory")]
public class HomeControllerTests
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Startup> _factory;
    public HomeControllerTests(CustomWebApplicationFactory<Startup> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Home/Index")]
    [InlineData("/Home/About")]
    [InlineData("/Home/Privacy")]
    [InlineData("/Home/Contacts")]
    [InlineData("/Home/Team")]
    public async Task GettingPageTest_ShouldRetunPage_WhenServiceIsRunning(string url)
    {
        // Arrange
        if (url.StartsWith('/') == false)
            throw new ArgumentException();

        //Act
        var defaultPage = await _client.GetAsync(url);
        var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

        // Assert
        defaultPage.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Children.Count().Should().BeGreaterThanOrEqualTo(1);
    }
}
