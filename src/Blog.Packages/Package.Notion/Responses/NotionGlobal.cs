﻿using Newtonsoft.Json;
using Package.Notion.Responses.Components.Properties;

namespace Package.Notion.Responses;

public class Database
{
    [JsonProperty("object")] public string Object { get; set; } = default!;

    [JsonProperty("results")] public List<Block> Results { get; set; } = default!;

    [JsonProperty("next_cursor")] public object NextCursor { get; set; } = default!;

    [JsonProperty("has_more")] public bool HasMore { get; set; }

    [JsonProperty("type")] public string Type { get; set; } = default!;

    [JsonProperty("page")] public object Page { get; set; } = default!;
}

public class Page
{
    [JsonProperty("object")] public string Object { get; set; } = default!;

    [JsonProperty("results")] public List<Block?> Results { get; set; } = default!;

    [JsonProperty("next_cursor")] public object NextCursor { get; set; } = default!;

    [JsonProperty("has_more")] public bool HasMore { get; set; }

    [JsonProperty("type")] public string Type { get; set; } = default!;

    [JsonProperty("block")] public object Block { get; set; } = default!;
}

public class Name
{
    [JsonProperty("id")] public string Id { get; set; } = default!;

    [JsonProperty("type")] public string Type { get; set; } = default!;

    [JsonProperty("title")] public List<Title> Title { get; set; } = default!;
}

public class Properties
{
}