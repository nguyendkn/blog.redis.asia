using Newtonsoft.Json;

namespace Blog.Service.NotionService.Responses.Components.Properties;

public class Paragraph
{
    [JsonProperty("rich_text")] public List<RichText> RichText { get; set; } = default!;

    [JsonProperty("color")] public string Color { get; set; } = default!;
}