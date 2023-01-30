namespace Package.Notion.Responses.Components.Properties;

public class TextLink
{
    public string Type { get; set; } = "url";
    public string Url { get; set; } = default!;
}