using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Package.Mongo.Abstracts;
using Package.Mongo.Metadata;

namespace Package.Mongo;

public static class MongoExtension
{
    public static void AddMongoContext<TMongoContext>(
        this IServiceCollection services,
        MongoConfigs mongoConfigs)
        where TMongoContext : MongoContext
    {
        services.AddSingleton(_ =>
        {
            var settings = MongoClientSettings.FromConnectionString(mongoConfigs.ConnectionString());
            var client = new MongoClient(settings);

            return client.GetDatabase(mongoConfigs.Database);
        });
        services.AddSingleton<IMongoContextBuilder>(provider =>
        {
            var requiredService = provider.GetRequiredService<IMongoDatabase>();
            return new MongoContextBuilder(requiredService);
        });
        services.AddSingleton<TMongoContext>();
    }

    public static BsonDocument ToBsonQuery<T>(this FilterDefinition<T> filter)
    {
        var serializerRegistry = BsonSerializer.SerializerRegistry;
        var documentSerializer = serializerRegistry.GetSerializer<T>();
        return filter.Render(documentSerializer, serializerRegistry);
    }

    public static IAggregateFluent<BsonDocument>? Search(this IAggregateFluent<BsonDocument> aggregateFluent, int take)
    {
        return aggregateFluent.Limit(take);
    }

    public static IAggregateFluent<BsonDocument>? Take(this IAggregateFluent<BsonDocument> aggregateFluent, int take)
    {
        return aggregateFluent.Limit(take);
    }

    public static IFindFluent<TEntity, TEntity> Take<TEntity>(this IFindFluent<TEntity, TEntity> fluent, int? limit)
    {
        return fluent.Limit(limit);
    }

    public static IFindFluent<TEntity, TEntity> OrderBy<TEntity>(this IFindFluent<TEntity, TEntity> fluent,
        Expression<Func<TEntity, object>> expression)
    {
        return fluent.SortBy(expression);
    }

    public static IFindFluent<TEntity, TEntity> OrderByDescending<TEntity>(this IFindFluent<TEntity, TEntity> fluent,
        Expression<Func<TEntity, object>> expression)
    {
        return fluent.SortByDescending(expression);
    }
}