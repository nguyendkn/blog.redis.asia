using Newtonsoft.Json;

namespace Blog.Service.NotionService.Responses.Components.Properties
{
    public class Select
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }
    }
}
