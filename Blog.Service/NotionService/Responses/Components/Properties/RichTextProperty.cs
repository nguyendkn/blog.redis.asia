using Newtonsoft.Json;

namespace Blog.Service.NotionService.Responses.Components.Properties;

public class RichTextProperty : Property
{
    public override PropertyType Type => PropertyType.RichText;

    [JsonProperty("rich_text")]
    public List<RichText> RichText { get; set; } = default!;
}