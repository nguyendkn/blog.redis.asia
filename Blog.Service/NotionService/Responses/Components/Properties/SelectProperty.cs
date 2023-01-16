using Newtonsoft.Json;

namespace Blog.Service.NotionService.Responses.Components.Properties
{
    public class SelectProperty : Property
    {
        public override PropertyType Type => PropertyType.Select;

        [JsonProperty("select")]
        public Select Select { get; set; } = default!;
    }
}
