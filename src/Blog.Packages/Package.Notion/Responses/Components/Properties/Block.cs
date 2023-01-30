using Newtonsoft.Json;

namespace Package.Notion.Responses.Components.Properties;

public class Block
{
    [JsonProperty("object")] public string Object { get; set; } = "block";

    [JsonProperty("id")] public string Id { get; set; } = default!;

    [JsonProperty("created_time")] public DateTime CreatedTime { get; set; }

    [JsonProperty("last_edited_time")] public DateTime LastEditedTime { get; set; }

    [JsonProperty("created_by")] public CreatedBy CreatedBy { get; set; } = default!;

    [JsonProperty("last_edited_by")] public LastEditedBy LastEditedBy { get; set; } = default!;

    [JsonProperty("has_children")] public bool HasChildren { get; set; }

    [JsonProperty("cover")] public Cover Cover { get; set; } = default!;

    [JsonProperty("icon")] public object Icon { get; set; } = default!;

    [JsonProperty("parent")] public Parent Parent { get; set; } = default!;

    [JsonProperty("archived")] public bool Archived { get; set; }

    [JsonProperty("type")] public string Type { get; set; } = default!;

    [JsonProperty("properties")] public Dictionary<string, Property> Properties { get; set; } = default!;

    [JsonProperty("url")] public string Url { get; set; } = default!;

    [JsonProperty("heading_2")] public Heading2 Heading2 { get; set; } = default!;
    [JsonProperty("heading_1")] public Heading1 Heading1 { get; set; } = default!;
    [JsonProperty("heading_3")] public Heading3 Heading3 { get; set; } = default!;

    [JsonProperty("image")] public Image Image { get; set; } = default!;

    [JsonProperty("paragraph")] public Paragraph Paragraph { get; set; } = default!;
    [JsonProperty("bulleted_list_item")] public BulletedListItem BulletedListItem { get; set; } = default!;
}