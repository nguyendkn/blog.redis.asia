using Newtonsoft.Json;

namespace Package.Notion.Responses.Components.Properties;

public class Text
{
    [JsonProperty("content")] public string Content { get; set; } = default!;

    [JsonProperty("link")] public TextLink? Link { get; set; }
}