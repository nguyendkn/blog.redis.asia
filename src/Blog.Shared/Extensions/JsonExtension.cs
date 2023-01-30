using Newtonsoft.Json;

namespace Blog.Shared.Extensions;

public static class JsonExtension
{
    public static string Serialize<T>(this T input)
    {
        return JsonConvert.SerializeObject(input);
    }

    public static T Deserialize<T>(this string input)
    {
        return !string.IsNullOrEmpty(input) ? JsonConvert.DeserializeObject<T>(input)! : default!;
    }

}