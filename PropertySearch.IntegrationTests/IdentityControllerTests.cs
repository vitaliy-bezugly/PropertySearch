using AngleSharp.Html.Dom;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using PropertySearch.IntegrationTests.Extensions;
using PropertySearch.IntegrationTests.Helpers;
using PropertySearchApp;
using PropertySearchApp.Common.Constants;
using PropertySearchApp.Models.Identities;
using System.Collections.Generic;
using System.Net;

namespace PropertySearch.IntegrationTests;

[Collection(name: "web-application-factory")]
public class IdentityControllerTests
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Startup> _factory;
    public IdentityControllerTests(CustomWebApplicationFactory<Startup> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true
        });
    }

    [Fact]
    public async Task RegisterUser_ShouldRegisterUser_WhenAllParametersAreValid()
    {
        // Arrange
        string url = ApplicationRoutes.Identity.Register;

        string password = "somestrongp4assword";
        var formData = new RegistrationFormViewModel
        {
            Username = "username",
            Email = "email@example.com",
            Password = password,
            ConfirmPassword = password,
            IsLandlord = false
        };

        //Act
        var defaultPage = await _client.GetAsync("/");
        var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

        var form = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("username", formData.Username),
            new KeyValuePair<string, string>("email", formData.Email),
            new KeyValuePair<string, string>("password", formData.Password),
            new KeyValuePair<string, string>("confirmPassword", formData.ConfirmPassword),
            new KeyValuePair<string, string>("isLandlord", "false")
        };
        var response = await _client.SendAsync(
            (IHtmlFormElement)content.QuerySelector("form[id='registration']"),
            (IHtmlButtonElement)content.QuerySelector("button[id='submitbtn']"),
             form);

        // Assert
        defaultPage.StatusCode.Should().Be(HttpStatusCode.OK);
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
    }
}
