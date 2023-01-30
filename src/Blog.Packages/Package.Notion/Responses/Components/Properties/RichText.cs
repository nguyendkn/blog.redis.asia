﻿using Newtonsoft.Json;

namespace Package.Notion.Responses.Components.Properties;

public class RichText
{
    public const string TypeText = "text";
    public const string TypeMention = "mention";
    public const string TypeLink = "link";
    public const string TypeEquation = "equation";
    
    [JsonProperty("type")] public string Type { get; set; } = default!;

    [JsonProperty("text")] public Text Text { get; set; } = default!;

    [JsonProperty("annotations")] public Annotations Annotations { get; set; } = default!;

    [JsonProperty("plain_text")] public string PlainText { get; set; } = default!;

    [JsonProperty("href")] public string? Href { get; set; }

    [JsonIgnore] public bool HasAttribute => Annotations?.HasAnnotation == true || !string.IsNullOrWhiteSpace(Href);

    [JsonIgnore] public bool HasStyle => !string.IsNullOrWhiteSpace(Annotations?.Color);
}