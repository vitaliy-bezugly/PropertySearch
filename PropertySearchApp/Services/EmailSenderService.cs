using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using PropertySearchApp.Common.Extensions;
using PropertySearchApp.Common.Logging;
using PropertySearchApp.Common.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PropertySearchApp.Services;

public class EmailSenderService : IEmailSender
{
    private readonly ILogger<EmailSenderService> _logger;

    public EmailSenderService(IOptions<AuthMessageSenderOptions> optionsAccessor, ILogger<EmailSenderService> logger)
    {
        Options = optionsAccessor.Value;
        _logger = logger;
    }

    public AuthMessageSenderOptions Options { get; } // Set with Secret Manager.

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(Options.SendGridKey))
        {
            throw new Exception("Null SendGridKey");
        }
        
        await Execute(Options.SendGridKey, subject, message, toEmail);
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
                .WithParameter(typeof(string).Name, nameof(toEmail), toEmail)
                .WithParameter(typeof(string).Name, nameof(subject), subject)
                .WithParameter(typeof(string).Name, nameof(message), message)
                .ToString());

            throw;
        }
    }
}