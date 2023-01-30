using System.Reflection;
using Blog.Shared.Extensions.HangfireExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Package.Mongo;
using Package.Notion;

namespace Blog.Shared.ValueObjects;

public class AppSettings
{
    public NotionConfigs NotionConfigs { get; set; } = default!;

    public RedisConfigs RedisConfigs { get; set; } = default!;

    public Hangfire Hangfire { get; set; } = default!;

    public MongoConfigs MongoConfigs { get; set; } = default!;
    
    public List<ScheduledTask> ScheduledTasks { get; set; } = default!;
    
    public static AppSettings Configuration<TProgram>(IHostEnvironment? environment = null)
    {
        var environmentName = environment?.EnvironmentName;
        var appSettingsName = string.IsNullOrEmpty(environmentName)
            ? "appsettings.dev.json"
            : $"appsettings.{environmentName}.json";
        Console.WriteLine(typeof(Assembly).Assembly.Location);
        var assemblyPath = Path.GetDirectoryName(typeof(TProgram).Assembly.Location);
        Console.WriteLine(assemblyPath);
        var appSettingsPath = Path.Combine(assemblyPath!, appSettingsName);
        var appSettings = new AppSettings();
        new ConfigurationBuilder()
            .AddJsonFile(appSettingsPath)
            .Build().Bind(appSettings);
        return appSettings;
    }

}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; } = default!;

    public string HangfireConnection { get; set; } = default!;
}

public class Hangfire
{
    public string UserName { get; set; } = default!;

    public string Password { get; set; } = default!;
}

public class RedisConfigs
{
    public string Host { get; set; } = default!;

    public int Port { get; set; }

    public string UserName { get; set; } = default!;

    public string Password { get; set; } = default!;
}