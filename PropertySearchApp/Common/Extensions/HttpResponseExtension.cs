namespace PropertySearchApp.Common.Extensions;

public static class HttpResponseExtension
{
    public static bool IsSuccessStatusCode(this HttpResponse response)
    {
        return (response.StatusCode >= 200) && (response.StatusCode <= 299);
    }
}