using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PropertySearch.IntegrationTests.Extensions;
using PropertySearch.IntegrationTests.Helpers;
using PropertySearchApp;
using PropertySearchApp.Models.Identities;
using PropertySearchApp.Persistence;

namespace PropertySearch.IntegrationTests;

[Collection(name: "web-application-factory")]
public abstract class AthenticationTestsBase
{
    protected readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Startup> _factory;
    public AthenticationTestsBase(CustomWebApplicationFactory<Startup> factory)
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

    protected async Task<(HttpResponseMessage, IHtmlDocument)> GetPageAsync(string url)
    {
        var defaultPage = await _client.GetAsync(url);
        var content = await HtmlHelpers.GetDocumentAsync(defaultPage);
        return (defaultPage, content);
    }
    protected async Task<HttpResponseMessage> SendRegistrationFormAsync(RegistrationFormViewModel registrationData, 
        IHtmlDocument content)
    {
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
        
        return response;
    }
    protected async Task<HttpResponseMessage> SendLoginFormAsync(LoginViewModel login,
    IHtmlDocument content)
    {
        var formData = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("inputUsername", login.Username),
            new KeyValuePair<string, string>("inputPassword", login.Password)
        };

        var form = (IHtmlFormElement)content.QuerySelector("form[id='login']");
        var submitButton = (IHtmlButtonElement)content.QuerySelector("button[id='submitbtn']");
        var response = await _client.SendAsync(form, submitButton, formData);

        return response;
    }
}
