using System.Security.Claims;

namespace PropertySearchApp.Controllers.Extensions;

public static class HttpContextAccessorExtension
{
    public static Guid GetUserId(this IHttpContextAccessor httpContextAccessor)
    {
        Guid id = Guid.Empty;
        string? claimValue = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(string.IsNullOrEmpty(claimValue))
        {
            throw new InvalidOperationException($"Can not get {ClaimTypes.NameIdentifier} from {httpContextAccessor}");
        }

        Guid.TryParse(claimValue, out id);
        return id;
    }
}
