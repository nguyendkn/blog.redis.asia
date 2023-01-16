using Newtonsoft.Json;

namespace Blog.Service.NotionService.Responses.Components.Properties;

public class Image
{
    public const string ExternalType = "external";
    public const string FileType = "file";
    
    [JsonProperty("caption")] public List<string> Caption { get; set; } = default!;

    [JsonProperty("type")] public string Type { get; set; } = default!;

    [JsonProperty("external")] public External? External { get; set; }
    
    [JsonProperty("file")] public File? File { get; set; }
}