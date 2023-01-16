using Blog.Shared.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Service.NotionService;

public static class NotionRegistration
{
    public static void AddNotionClient(this IServiceCollection services, AppSettings appSettings)
    {
        var notionClient = new NotionClient(appSettings.NotionConfigs);
        services.AddSingleton<INotionClient>(notionClient);
    }
}