using Newtonsoft.Json;

namespace Package.Notion.Responses.Components.Properties
{
    public class FileProperty : Property
    {
        public override PropertyType Type => PropertyType.Files;

        [JsonProperty("files")]
        public List<File> Files { get; set; } = default!;
    }
}
