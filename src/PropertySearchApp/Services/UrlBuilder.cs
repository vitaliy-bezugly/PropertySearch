using PropertySearchApp.Common.Constants;

namespace PropertySearchApp.Services;

public class UrlBuilder
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UrlBuilder> _logger;

    public UrlBuilder(IHttpContextAccessor httpContextAccessor, ILogger<UrlBuilder> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public string BuildUrlForEmailConfirmation(Guid userId, string token)
    {
        if (_httpContextAccessor.HttpContext is null)
            throw new InvalidOperationException("Can not build url. HttpContext is null");
        
        var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
        baseUrl += $"/{ApplicationRoutes.Identity.EmailConfirmationResult}";

        string emailConfirmationUrl = baseUrl + "?" + BuildQueryBasedOnUserIdAndToken(userId, token);
        _logger.LogInformation($"Email confirmation URL: {emailConfirmationUrl}");
        return emailConfirmationUrl;
    }

    private string BuildQueryBasedOnUserIdAndToken(Guid userId, string token)
    {
        return $"userId={userId}&token={token}";
    }
}