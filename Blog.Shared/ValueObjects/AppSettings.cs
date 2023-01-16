using Google.Api;

namespace Blog.Shared.ValueObjects;

public class AppSettings
{
    public NotionConfigs NotionConfigs { get; set; } = default!;

    public RedisConfigs RedisConfigs { get; set; } = default!;
}

public class NotionConfigs
{
    public string API { get; set; } = default!;

    public string Token { get; set; } = default!;

    public string Version { get; set; } = default!;
}

public class RedisConfigs
{
    public string Host { get; set; } = default!;

    public int Port { get; set; }

    public string UserName { get; set; } = default!;

    public string Password { get; set; } = default!;
}