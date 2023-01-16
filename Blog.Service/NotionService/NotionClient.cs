using Blog.Service.NotionService.Responses;
using Blog.Service.NotionService.Responses.Components.Properties;
using Blog.Shared.ValueObjects;
using Newtonsoft.Json;
using RestSharp;

namespace Blog.Service.NotionService;

public interface INotionClient
{
    Task<List<Dictionary<string, object>>> QueryAsync(string database);
    Task<List<Dictionary<string, object>>> ConfigsQueryAsync(string database);
    Task<Page?> GetPageAsync(string page);
}

public class NotionClient : INotionClient
{
    private readonly NotionConfigs _notionConfigs;
    
    public NotionClient(NotionConfigs notionConfigs)
    {
        _notionConfigs = notionConfigs;
    }
    
    public async Task<List<Dictionary<string, object>>> QueryAsync(string database)
    {
        var client = new RestClient(_notionConfigs.API);
        var dictionaries = new List<Dictionary<string, object>>();
        var request = new RestRequest($"databases/{database}/query", Method.Post);
        request.AddHeader("Authorization", $"Bearer {_notionConfigs.Token}");
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Notion-Version", _notionConfigs.Version);
        request.AddParameter("application/json", string.Empty, ParameterType.RequestBody);
        var response = await client.ExecuteAsync(request);

        if (string.IsNullOrEmpty(response.Content)) return default!;
        var databaseResponse = JsonConvert.DeserializeObject<Database>(response.Content);
        var blocks = databaseResponse?.Results;
        if (blocks is null) return default!;
        foreach (var block in blocks)
        {
            var dictionary = new Dictionary<string, object>
            {
                { "Id", block.Id }, { "CreatedTime", block.CreatedTime }, { "LastEditedTime", block.LastEditedTime }
            };
            foreach (var property in block.Properties)
            {
                switch (property.Value)
                {
                    case SelectProperty selectProperty:
                        var category = "";
                        if (selectProperty.Select != null)
                        {
                            category = selectProperty.Select.Name;
                        }

                        dictionary.Add(property.Key, category);
                        break;
                    case FileProperty fileProperty:
                        var images = "";
                        if (fileProperty.Files.Count > 0)
                        {
                            foreach (var item in fileProperty.Files)
                            {
                                if (!string.IsNullOrEmpty(item.SubFile.Url))
                                {
                                    images = images + item.SubFile.Url + ",";
                                }
                            }
                        }

                        dictionary.Add(property.Key, images);
                        break;
                    case RichTextProperty richTextProperty:
                        var textContent = "";
                        if (richTextProperty.RichText.Count > 0)
                        {
                            textContent = richTextProperty.RichText.Aggregate(textContent,
                                (current, richText) => current + richText.Text.Content);
                        }

                        dictionary.Add(property.Key, textContent);
                        break;
                    case TitleProperty titleProperty:
                        var content = titleProperty.Title.FirstOrDefault()?.Text.Content;
                        dictionary.Add(property.Key, content ?? string.Empty);
                        break;
                    case RelationProperty relationProperty:
                        var value = relationProperty.Relation.FirstOrDefault()?.Id;
                        if (value != null)
                            dictionary.Add(property.Key, value);
                        break;
                }
            }

            dictionaries.Add(dictionary);
        }

        return dictionaries;
    }

    public async Task<List<Dictionary<string, object>>> ConfigsQueryAsync(string database)
    {
        var client = new RestClient(_notionConfigs.API);
        var dictionaries = new List<Dictionary<string, object>>();
        var request = new RestRequest($"databases/{database}/query", Method.Post);
        request.AddHeader("Authorization", $"Bearer {_notionConfigs.Token}");
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Notion-Version", _notionConfigs.Version);
        request.AddParameter("application/json", string.Empty, ParameterType.RequestBody);
        var response = await client.ExecuteAsync(request);

        if (string.IsNullOrEmpty(response.Content)) return default!;
        var databaseResponse = JsonConvert.DeserializeObject<Database>(response.Content);
        var blocks = databaseResponse?.Results;
        if (blocks is null) return default!;
        foreach (var block in blocks)
        {
            var dictionary = new Dictionary<string, object>
            {
                { "Id", block.Id }, { "CreatedTime", block.CreatedTime }, { "LastEditedTime", block.LastEditedTime }
            };
            foreach (var property in block.Properties)
            {
                switch (property.Value)
                {
                    case SelectProperty selectProperty:
                        var type = "";
                        if (selectProperty.Select != null)
                        {
                            type = selectProperty.Select.Name;
                        }

                        dictionary.Add(property.Key, type);
                        break;
                    case FileProperty fileProperty:
                        var images = "";
                        if (fileProperty.Files.Count > 0)
                        {
                            images = fileProperty.Files.Where(item => !string.IsNullOrEmpty(item.SubFile.Url))
                                .Aggregate(images, (current, item) => current + item.SubFile.Url + ",");
                        }

                        dictionary.Add(property.Key, images);
                        break;
                    case RichTextProperty richTextProperty:
                        var textContent = "";
                        if (richTextProperty.RichText.Count > 0)
                        {
                            textContent = richTextProperty.RichText.Aggregate(textContent,
                                (current, richText) => current + richText.Text.Content);
                        }

                        dictionary.Add(property.Key, textContent);
                        break;
                    case TitleProperty titleProperty:
                        var content = titleProperty.Title.FirstOrDefault()?.Text.Content;
                        dictionary.Add(property.Key, content ?? string.Empty);
                        break;
                    case RelationProperty relationProperty:
                        var value = relationProperty.Relation.FirstOrDefault()?.Id;
                        if (value != null)
                            dictionary.Add(property.Key, value);
                        break;
                }
            }

            dictionaries.Add(dictionary);
        }

        return dictionaries;
    }

    public async Task<Page?> GetPageAsync(string page)
    {
        var client = new RestClient();
        var request = new RestRequest(new Uri($"{_notionConfigs.API}/blocks/{page}/children"));
        request.AddHeader("Authorization", $"Bearer {_notionConfigs.Token}");
        request.AddHeader("Notion-Version", "2022-02-22");
        request.AddParameter("application/json", string.Empty, ParameterType.RequestBody);
        var response = await client.ExecuteAsync(request);
        return !string.IsNullOrEmpty(response.Content)
            ? JsonConvert.DeserializeObject<Page>(response.Content)
            : default!;
    }
}