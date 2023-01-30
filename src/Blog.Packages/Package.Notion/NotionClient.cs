using Newtonsoft.Json;
using Package.Notion.Responses;
using Package.Notion.Responses.Components.Properties;
using RestSharp;

namespace Package.Notion;

public interface INotionClient
{
    Task<List<Dictionary<string, object>>> QueryAsync(NotionDatabase database);
    Task<List<Dictionary<string, object>>> ConfigsQueryAsync(NotionDatabase database);
    Task<Page?> GetPageAsync(string page);
}

public class NotionClient : INotionClient
{
    private readonly NotionConfigs _notionConfigs;

    public NotionClient(NotionConfigs notionConfigs)
    {
        _notionConfigs = notionConfigs;
    }

    public async Task<List<Dictionary<string, object>>> QueryAsync(NotionDatabase database)
    {
        var client = new RestClient(_notionConfigs.API);
        var dictionaries = new List<Dictionary<string, object>>();
        var request = new RestRequest($"databases/{database.Id}/query", Method.POST);
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
                { "Id", block.Id },
                { "CreatedTime", block.CreatedTime },
                { "LastEditedTime", block.LastEditedTime },
                { "Cover", block.Cover?.File?.Url ?? string.Empty }
            };
            foreach (var property in block.Properties)
            {
                switch (property.Value)
                {
                    case SelectProperty selectProperty:
                        var selection = "";
                        if (selectProperty.Select != null)
                        {
                            selection = selectProperty.Select.Name;
                        }

                        dictionary.Add(property.Key, selection);
                        break;
                    case MultiSelectProperty multiSelectProperty:
                        var selections = "";
                        if (multiSelectProperty.MultiSelect.Any())
                        {
                            var selectItems = multiSelectProperty.MultiSelect.Select(x => x.Name);
                            selections = string.Join(',', selectItems);
                        }

                        dictionary.Add(property.Key, selections);
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

    public async Task<List<Dictionary<string, object>>> ConfigsQueryAsync(NotionDatabase database)
    {
        var client = new RestClient(_notionConfigs.API);
        var dictionaries = new List<Dictionary<string, object>>();
        var request = new RestRequest($"databases/{database.Id}/query", Method.POST);
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