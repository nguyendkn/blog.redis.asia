namespace Package.Mongo;

public class MongoConfigs
{
    public string Host { get; set; } = default!;

    public string Database { get; set; } = default!;

    public string UserName { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string ConnectionString()
    {
        return $"mongodb+srv://{UserName}:{Password}@{Host}/{Database}";
    }
}