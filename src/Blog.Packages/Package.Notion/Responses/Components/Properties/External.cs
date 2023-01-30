using Newtonsoft.Json;

namespace Package.Notion.Responses.Components.Properties;

public class External
{
    [JsonProperty("url")] public string Url { get; set; } = default!;
}