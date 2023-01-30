using MongoDB.Driver;
using Package.Mongo.Metadata;

namespace Package.Mongo.Abstracts;

public interface IMongoContextBuilder
{
    IMongoDatabase Database { get; }

    void Entity<TEntity>(Action<EntityTypeBuilder<TEntity>> action) where TEntity : class;

    void OnConfiguring(MongoContext context);

    void OnModelCreating(Action action);
}