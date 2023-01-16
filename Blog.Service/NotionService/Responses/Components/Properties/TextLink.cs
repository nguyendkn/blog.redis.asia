namespace Blog.Service.NotionService.Responses.Components.Properties;

public class TextLink
{
    public string Type { get; set; } = "url";
    public string Url { get; set; } = default!;
}