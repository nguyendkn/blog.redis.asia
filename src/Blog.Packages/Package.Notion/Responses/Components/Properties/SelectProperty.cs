using Newtonsoft.Json;

namespace Package.Notion.Responses.Components.Properties
{
    public class SelectProperty : Property
    {
        public override PropertyType Type => PropertyType.Select;

        [JsonProperty("select")]
        public Select Select { get; set; } = default!;
    }
}
