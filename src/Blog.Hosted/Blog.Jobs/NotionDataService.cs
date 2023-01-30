using Blog.Domain.Aggregates.PostAggregate;
using Blog.Domain.DbContexts;
using Blog.Shared.Extensions;
using Blog.Shared.Extensions.HangfireExtensions;
using Blog.Shared.ValueObjects;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Package.Notion;

namespace Blog.Jobs;

public class NotionDataService : ICronJob
{
    private readonly AppSettings _appSettings;
    private readonly BlogContext _context;
    private readonly INotionClient _notionClient;
    private readonly ILogger<NotionDataService> _logger;

    public NotionDataService(AppSettings appSettings,
        INotionClient notionClient, BlogContext context,
        ILogger<NotionDataService> logger)
    {
        _appSettings = appSettings;
        _notionClient = notionClient;
        _context = context;
        _logger = logger;
    }

    public async Task<string> Run()
    {
        var database = _appSettings.NotionConfigs.Database;
        await SyncPosts(database.FirstOrDefault(x => x.Name.Equals("Posts")));
        return await Task.FromResult("OK");
    }

    private async Task SyncPosts(NotionDatabase? database)
    {
        if (database is null) return;
        var dataPost = await _notionClient.QueryAsync(database);
        var posts = new List<string>();
        foreach (var dictionary in dataPost)
        {
            var id = dictionary.FirstOrDefault(x => x.Key.Equals("Id")).Value.ToString();
            if (!string.IsNullOrEmpty(id)) posts.Add(id);
            var title = dictionary.GetValue("Title");
            var createdTime = DateTime.Parse(dictionary.GetValue("CreatedTime"));
            var lastEditedTime = DateTime.Parse(dictionary.GetValue("LastEditedTime"));
            var content = await GetContent(id);
            var category = dictionary.GetValue("Category");
            var cover = dictionary.GetValue("Cover");
            var tags = dictionary.GetValue("Tags").Split(',').ToList();

            var any = await _context.Posts.AnyAsync(x => x.Id.Equals(id!.ToGuid()));
            if (!string.IsNullOrEmpty(id) && any)
            {
                var post = await _context.Posts
                    .FirstOrDefaultAsync(x => x.Id.Equals(id.ToGuid()));
                if (post is null) return;

                post.Update(title, cover, createdTime, lastEditedTime, content, category, tags);
                await _context.Posts.UpdateAsync(post.Id, post);
                _logger.LogInformation("{Time} Modified entity: {EntityId}", DateTime.Now, id);
            }
            else
            {
                if (string.IsNullOrEmpty(id)) continue;
                await _context.Posts.AddAsync(new Post
                {
                    Id = id.ToGuid(),
                    Title = title,
                    Content = content,
                    Category = category,
                    LastEditedTime = lastEditedTime,
                    Deleted = false
                });
                _logger.LogInformation("{Time} Created entity: {EntityId}", DateTime.Now, id);
            }
        }

        var postsToDelete = await _context.Posts.Where(x => !posts.Any(y => x.Id.ToString().Equals(y))).ToListAsync();
        foreach (var post in postsToDelete)
        {
            post.Deleted = true;
        }

        await _context.Posts.UpdateAsync(postsToDelete);
    }

    private async Task<string> GetContent(string? pageId)
    {
        if (string.IsNullOrEmpty(pageId)) return string.Empty;
        var page = await _notionClient.GetPageAsync(pageId);
        var content = page.ToHtml(out var indexes);
        return content;
    }
}