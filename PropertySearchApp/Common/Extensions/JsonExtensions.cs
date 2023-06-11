using Newtonsoft.Json;

namespace PropertySearchApp.Common.Extensions;

public static class JsonExtensions
{
    public static string SerializeObject<T>(this T value)
    {
        return JsonConvert.SerializeObject(value);
    }
}