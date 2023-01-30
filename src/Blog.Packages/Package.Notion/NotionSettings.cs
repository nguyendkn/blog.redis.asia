namespace Package.Notion;

public class NotionConfigs
{
    public string API { get; set; } = default!;

    public string Token { get; set; } = default!;

    public string Version { get; set; } = default!;

    public List<NotionDatabase> Database { get; set; } = default!;
}

public class NotionDatabase
{
    public string Name { get; set; } = default!;

    public string Id { get; set; } = default!;
}