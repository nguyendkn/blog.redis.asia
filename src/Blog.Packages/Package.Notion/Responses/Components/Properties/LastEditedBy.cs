using Newtonsoft.Json;

namespace Package.Notion.Responses.Components.Properties;

public class LastEditedBy
{
    [JsonProperty("object")] public string Object { get; set; } = default!;

    [JsonProperty("id")] public string Id { get; set; } = default!;
}