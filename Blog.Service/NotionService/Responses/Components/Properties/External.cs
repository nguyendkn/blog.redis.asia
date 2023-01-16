using Newtonsoft.Json;

namespace Blog.Service.NotionService.Responses.Components.Properties;

public class External
{
    [JsonProperty("url")] public string Url { get; set; } = default!;
}