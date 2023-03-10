using Newtonsoft.Json;

namespace Package.Notion.Responses.Components.Properties;

public class Href
{
    [JsonProperty("id")] public string Id { get; set; } = default!;

    [JsonProperty("type")] public string Type { get; set; } = default!;

    [JsonProperty("rich_text")] public List<RichText> RichText { get; set; } = default!;
}