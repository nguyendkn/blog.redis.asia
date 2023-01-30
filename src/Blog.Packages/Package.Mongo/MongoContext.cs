using MongoDB.Driver;
using Package.Mongo.Abstracts;

namespace Package.Mongo;

public class MongoContext
{
    protected readonly IMongoContextBuilder ModelBuilder;
    public IMongoDatabase Database => ModelBuilder.Database;

    protected MongoContext(IMongoContextBuilder modelBuilder)
    {
        ModelBuilder = modelBuilder;
        modelBuilder.OnConfiguring(this);
        modelBuilder.OnModelCreating(OnModelCreating);
    }

    protected virtual void OnModelCreating()
    {
    }
}