using MongoDB.Bson.Serialization.Attributes;
using Package.Mongo.Attributes;

namespace Package.Mongo;

public class MongoDocument
{
    [BsonId] [MongoKey] public Guid Id { get; set; } = Guid.NewGuid();
}