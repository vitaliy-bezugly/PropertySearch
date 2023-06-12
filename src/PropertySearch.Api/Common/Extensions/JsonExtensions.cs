using Newtonsoft.Json;

namespace PropertySearch.Api.Common.Extensions;

public static class JsonExtensions
{
    public static string SerializeObject<T>(this T value)
    {
        return JsonConvert.SerializeObject(value);
    }
}