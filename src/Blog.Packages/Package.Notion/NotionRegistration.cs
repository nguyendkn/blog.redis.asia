using Microsoft.Extensions.DependencyInjection;

namespace Package.Notion;

public static class NotionRegistration
{
    public static void AddNotionClient(this IServiceCollection services, NotionConfigs notionConfigs)
    {
        var notionClient = new NotionClient(notionConfigs);
        services.AddSingleton<INotionClient>(notionClient);
    }
}