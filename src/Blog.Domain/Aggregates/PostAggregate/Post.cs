using Blog.Shared.Extensions;
using Package.Mongo;

namespace Blog.Domain.Aggregates.PostAggregate;

public class Post : MongoDocument
{
    public bool Deleted { get; set; }

    public string? Cover { get; set; }

    public string Title { get; set; } = default!;

    public DateTime CreatedTime { get; set; }

    public DateTime LastEditedTime { get; set; }

    public string Content { get; set; } = default!;

    public string Category { get; set; } = default!;

    public string Slug { get; set; } = default!;

    public List<string>? Tags { get; set; }

    public void Update(string title, string cover,
        DateTime createdTime, DateTime lastEditedTime, string content,
        string category, List<string>? tags)
    {
        Title = title;
        Cover = cover;
        CreatedTime = createdTime;
        LastEditedTime = lastEditedTime;
        Content = content;
        Category = category;
        Tags = tags ?? new List<string>();
        Slug = GetSlug();
    }

    public string GetSlug()
    {
        return $"{Title.Slugable()}-{CreatedTime:yyyyMMddHHmm}";
    }
}