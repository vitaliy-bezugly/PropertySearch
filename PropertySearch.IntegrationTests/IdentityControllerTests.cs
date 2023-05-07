using AngleSharp.Html.Dom;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PropertySearch.IntegrationTests.Extensions;
using PropertySearch.IntegrationTests.Helpers;
using PropertySearchApp;
using PropertySearchApp.Common.Constants;
using PropertySearchApp.Models.Identities;
using PropertySearchApp.Persistence;
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

        SeedData(_factory.Services);
    }

    private void SeedData(IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();

            var roles = db.Roles.ToList();
            if (roles.Count < 3)
            {
                db.Roles.Add(new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "user", NormalizedName = "USER", ConcurrencyStamp = Guid.NewGuid().ToString() });
                db.Roles.Add(new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "admin", NormalizedName = "ADMIN", ConcurrencyStamp = Guid.NewGuid().ToString() });
                db.Roles.Add(new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "landlord", NormalizedName = "LANDLORD", ConcurrencyStamp = Guid.NewGuid().ToString() });

                db.SaveChanges();
            }
        }
    }

    [Fact]
    public async Task RegisterUser_ShouldRegisterUser_WhenAllParametersAreValid()
    {
        // Arrange
        string url = ApplicationRoutes.Identity.Register;

        string password = "somestrongp4assword";
        var registrationData = new RegistrationFormViewModel
        {
            Username = "username",
            Email = "email@example.com",
            Password = password,
            ConfirmPassword = password,
            IsLandlord = false
        };

        //Act
        var defaultPage = await _client.GetAsync(url);
        var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

        var formData = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("inputUsername", registrationData.Username),
            new KeyValuePair<string, string>("inputEmail", registrationData.Email),
            new KeyValuePair<string, string>("inputPassword", registrationData.Password),
            new KeyValuePair<string, string>("repeatPassword", registrationData.ConfirmPassword)
        };

        var form = (IHtmlFormElement)content.QuerySelector("form[id='registration']");
        var submitButton = (IHtmlButtonElement)content.QuerySelector("button[id='submitbtn']");
        var response = await _client.SendAsync(form, submitButton, formData);

        // Assert
        defaultPage.StatusCode.Should().Be(HttpStatusCode.OK);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
