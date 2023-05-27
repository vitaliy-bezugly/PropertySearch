using Newtonsoft.Json;

namespace PropertySearchApp.Common.Extensions;

public static class JsonExtension
{
    public static string SerializeObject<T>(this T value)
    {
        return JsonConvert.SerializeObject(value);
    }
}