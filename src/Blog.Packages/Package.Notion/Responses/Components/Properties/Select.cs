using Newtonsoft.Json;

namespace Package.Notion.Responses.Components.Properties
{
    public class Select
    {
        [JsonProperty("id")] public string Id { get; set; } = default!;

        [JsonProperty("name")] public string Name { get; set; } = default!;

        [JsonProperty("color")] public string Color { get; set; } = default!;
    }
}
