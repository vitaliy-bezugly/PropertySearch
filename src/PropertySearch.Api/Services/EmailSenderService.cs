using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using PropertySearch.Api.Common.Logging;
using PropertySearch.Api.Common.Options;
using PropertySearch.Api.Common.Extensions;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PropertySearch.Api.Services;

public class EmailSenderService : IEmailSender
{
    private readonly ILogger<EmailSenderService> _logger;
    private readonly AuthMessageSenderOptions _options; // Set with Secret Manager.

    public EmailSenderService(IOptions<AuthMessageSenderOptions> optionsAccessor, ILogger<EmailSenderService> logger)
    {
        _options = optionsAccessor.Value;
        _logger = logger;
    }
    
    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(_options.SendGridKey))
        {
            throw new Exception("Null SendGridKey");
        }
        
        await Execute(_options.SendGridKey, subject, message, toEmail);
    }

    private async Task Execute(string apiKey, string subject, string message, string toEmail)
    {
        try
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("bezuglivita123@gmail.com", "Property Search Application"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
        
            msg.AddTo(new EmailAddress(toEmail));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);
            
            _logger.LogInformation($"Response for send email: {response.SerializeObject()}");
            
            if(response.IsSuccessStatusCode)
                _logger.LogInformation($"Email to {toEmail} queued successfully!");
            else
                _logger.LogWarning( $"Failure Email to {toEmail}");
        }
        catch (Exception e)
        {
            _logger.LogWarning( $"Failure Email to {toEmail}");
            _logger.LogCritical(new LogEntry()
                .WithClass(nameof(EmailSenderService))
                .WithMethod(nameof(Execute))
                .WithUnknownOperation()
                .WithComment(e.Message)
                .WithParameter(nameof(String), nameof(toEmail), toEmail)
                .WithParameter(nameof(String), nameof(subject), subject)
                .WithParameter(nameof(String), nameof(message), message)
                .ToString());

            throw;
        }
    }
}