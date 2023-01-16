﻿using Newtonsoft.Json;

namespace Blog.Service.NotionService.Responses.Components.Properties;

public class Heading2
{
    [JsonProperty("rich_text")] public List<RichText> RichText { get; set; } = default!;

    [JsonProperty("is_toggleable")] public bool IsToggleable { get; set; }

    [JsonProperty("color")] public string Color { get; set; } = default!;
}