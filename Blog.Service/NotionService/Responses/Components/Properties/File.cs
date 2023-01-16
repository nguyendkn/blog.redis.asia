using Newtonsoft.Json;

namespace Blog.Service.NotionService.Responses.Components.Properties;

public class File
{
    [JsonProperty("url")]
    public string Url { get; set; } = default!;

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
