using Newtonsoft.Json;

namespace Package.Notion.Responses.Components.Properties;

public class File
{
    [JsonProperty("url")]
    public string? Url { get; set; }

    [JsonProperty("file")]
    public SubFile SubFile { get; set; } = default!;
}

public class SubFile
{
    [JsonProperty("url")]
    public string Url { get; set; } = default!;

    [JsonProperty("expiry_time")]
    public DateTime ExpiryTime { get; set; }
}
