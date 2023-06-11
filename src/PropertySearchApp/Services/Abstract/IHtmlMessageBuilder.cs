namespace PropertySearchApp.Services.Abstract;

public interface IHtmlMessageBuilder
{
    string BuildEmailConfirmationMessage(Guid userId, string username, string token);
}