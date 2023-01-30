using Newtonsoft.Json;

namespace Package.Notion.Responses.Components.Properties;

public class MultiSelectProperty : Property
{
    public override PropertyType Type => PropertyType.MultiSelect;
    
    [JsonProperty("multi_select")]
    public List<Select> MultiSelect { get; set; } = default!;
}