using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Services;

public class HtmlMessageBuilder : IHtmlMessageBuilder
{
    private readonly UrlBuilder _urlBuilder;

    public HtmlMessageBuilder(UrlBuilder urlBuilder)
    {
        _urlBuilder = urlBuilder;
    }

    public string BuildEmailConfirmationMessage(Guid userId, string username, string token) 
    {
        string message = string.Empty;
        string emailConfirmationUrl = _urlBuilder.BuildUrlForEmailConfirmation(userId, token);
        
        message += "<h1>Email Confirmation</h1>";
        message += $@"<p>Dear {username}</p>";
        
        message += "<p>Thank you for signing up with our service. To complete the registration process, please click the button below to confirm your email address:</p>";
        message += $"<p><a href=\"{emailConfirmationUrl}\" style=\"background-color: #4CAF50; color: white; padding: 12px 20px; text-decoration: none; display: inline-block; border-radius: 4px;\">Confirm Email Address</a></p>";
    
        message += "<p>If the button above does not work, you can also copy and paste the following link into your web browser:</p>";
        message += $"<p>{emailConfirmationUrl}</p>";
    
        message += "<p>Thank you for choosing our service. If you have any questions or need further assistance, please don't hesitate to contact our support team.</p>";

        message += "<p>Best regards</p>";
        message += "<p>Property Search Team</p>";

        return message;
    }
}