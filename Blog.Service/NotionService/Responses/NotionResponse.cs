using Newtonsoft.Json;

namespace Blog.Service.NotionService.Responses;

public interface INotionParameters
{
    [JsonProperty("start_cursor")] string StartCursor { get; set; }

    [JsonProperty("page_size")] int? PageSize { get; set; }
}