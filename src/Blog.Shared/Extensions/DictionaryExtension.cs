namespace Blog.Shared.Extensions;

public static class DictionaryExtension
{
    public static string GetValue<TValue>(this Dictionary<string, TValue> dictionary, string key)
    {
        var parsed = dictionary.TryGetValue(key, out var value);
        return (parsed && value != null) ? value.ToString()! : string.Empty;
    }
}