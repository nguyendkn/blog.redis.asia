using Newtonsoft.Json;

namespace Package.Notion.Responses.Components.Properties;

public class TitleProperty : Property
{
    public override PropertyType Type => PropertyType.Title;

    [JsonProperty("title")]
    public List<Title> Title { get; set; } = default!;
}