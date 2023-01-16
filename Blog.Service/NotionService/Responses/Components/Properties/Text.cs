using Newtonsoft.Json;

namespace Blog.Service.NotionService.Responses.Components.Properties;

public class Text
{
    [JsonProperty("content")] public string Content { get; set; } = default!;

    [JsonProperty("link")] public TextLink? Link { get; set; }
}