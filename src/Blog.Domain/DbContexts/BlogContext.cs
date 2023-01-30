using Blog.Domain.Aggregates.PostAggregate;
using Package.Mongo;
using Package.Mongo.Abstracts;

namespace Blog.Domain.DbContexts;

public class BlogContext : MongoContext
{
    public BlogContext(IMongoContextBuilder modelBuilder) : base(modelBuilder)
    {
    }

    public MongoSet<Post> Posts { get; set; } = default!;
}