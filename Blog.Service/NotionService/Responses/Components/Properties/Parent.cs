using Newtonsoft.Json;

namespace Blog.Service.NotionService.Responses.Components.Properties;

public class Parent
{
    [JsonProperty("type")] public string Type { get; set; } = default!;

    [JsonProperty("database_id")] public string DatabaseId { get; set; } = default!;
}