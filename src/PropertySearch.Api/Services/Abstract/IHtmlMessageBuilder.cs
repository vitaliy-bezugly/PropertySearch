namespace PropertySearch.Api.Services.Abstract;

public interface IHtmlMessageBuilder
{
    string BuildEmailConfirmationMessage(Guid userId, string username, string token);
}