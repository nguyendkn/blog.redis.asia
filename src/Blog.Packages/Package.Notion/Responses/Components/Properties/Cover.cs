using Newtonsoft.Json;

namespace Package.Notion.Responses.Components.Properties;

public class Cover
{
    [JsonProperty("type")] public string Type { get; set; } = default!;

    [JsonProperty("file")] public File File { get; set; } = default!;
}