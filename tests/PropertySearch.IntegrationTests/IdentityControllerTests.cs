using FluentAssertions;
using PropertySearch.Api;
using PropertySearch.Api.Common.Constants;
using PropertySearch.Api.Models.Identities;
using System.Net;

namespace PropertySearch.IntegrationTests;

[Collection(name: "web-application-factory")]
public class IdentityControllerTests : AthenticationTestsBase
{
    public IdentityControllerTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
    { }

    [Fact]
    public async Task GetRegisterPage_ShouldReturnPage_WhenUrlIsCorrect()
    {
        // Arrange
        string registerUrl = ApplicationRoutes.Identity.Register;

        //Act
        var result = await GetPageAsync(registerUrl);
        var registerPageResponseMessage = result.Item1;

        // Assert
        registerPageResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    [Fact]
    public async Task GetLoginPage_ShouldReturnPage_WhenUrlIsCorrect()
    {
        // Arrange
        string loginUrl = ApplicationRoutes.Identity.Login;

        //Act
        var loginPage = await GetPageAsync(loginUrl);
        var loginPageResponseMessage = loginPage.Item1;

        // Assert
        loginPageResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task RegisterUser_ShouldRegisterUser_WhenAllParametersAreValid()
    {
        // Arrange
        string url = ApplicationRoutes.Identity.Register;
        string password = "some-strong-password44";
        var registrationData = new RegistrationFormViewModel
        {
            Username = "username",
            Email = "email@example.com",
            Password = password,
            ConfirmPassword = password,
            IsLandlord = false
        };

        //Act
        var result = await GetPageAsync(url);
        var pageResponseMessage = result.Item1;
        var content = result.Item2;

        var response = await SendRegistrationFormAsync(registrationData, content);

        // Assert
        pageResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Theory]
    [InlineData(null, "email@example.com", "password", "password", false)]
    [InlineData("username", null, "password", "password", false)]
    [InlineData("username", "", "password", "password", false)]
    [InlineData("username", "invalid", "password", "password", false)]
    [InlineData("username", "email@example.com", null, "null", false)]
    [InlineData("username", "email@example.com", "", "", false)]
    [InlineData("username", "email@example.com", "inv", "inv", false)]
    [InlineData("username", "email@example.com", "valid", "N0tEqual", false)]
    public async Task RegisterUser_ShouldNotRegisterUser_WhenParametersAreInvalid(
        string username, string email, string password, string confirm, bool landlord)
    {
        // Arrange
        string url = ApplicationRoutes.Identity.Register;
        var registrationData = new RegistrationFormViewModel
        {
            Username = username,
            Email = email,
            Password = password,
            ConfirmPassword = confirm,
            IsLandlord = landlord
        };

        //Act
        var result = await GetPageAsync(url);
        var pageResponseMessage = result.Item1;
        var content = result.Item2;

        var response = await SendRegistrationFormAsync(registrationData, content);

        // Assert
        pageResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        response.StatusCode.Should().NotBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task LoginUser_ShouldLoginUser_WhenUserExists()
    {
        // Arrange
        string registerUrl = ApplicationRoutes.Identity.Register, loginUrl = ApplicationRoutes.Identity.Login;
        string password = "some-strong-password44";
        var registrationData = new RegistrationFormViewModel
        {
            Username = "username",
            Email = "email@example.com",
            Password = password,
            ConfirmPassword = password,
            IsLandlord = false
        };
        var loginData = new LoginViewModel
        {
            Username = "username",
            Password = password
        };

        //Act
        var result = await GetPageAsync(registerUrl);
        var registerPageResponseMessage = result.Item1;
        var registerContent = result.Item2;

        var registerResponse = await SendRegistrationFormAsync(registrationData, registerContent);
        
        var loginPage = await GetPageAsync(loginUrl);
        var loginPageResponseMessage = loginPage.Item1;
        var loginContent = loginPage.Item2;
        var loginResponse = await SendLoginFormAsync(loginData, loginContent);
        
        // Assert
        registerPageResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        loginPageResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

        registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
