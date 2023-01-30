using Newtonsoft.Json;

namespace Package.Notion.Responses.Components.Properties;

public class Relation
{
    [JsonProperty("id")]
    public string Id { get; set; } = default!;

    [JsonProperty("type")]
    public string Type { get; set; } = default!;

    [JsonProperty("relation")]
    public List<string> Relations { get; set; } = default!;
}