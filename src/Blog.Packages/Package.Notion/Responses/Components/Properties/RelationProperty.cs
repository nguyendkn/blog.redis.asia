using Newtonsoft.Json;

namespace Package.Notion.Responses.Components.Properties;

public class RelationProperty : Property
{
    public override PropertyType Type => PropertyType.Relation;

    [JsonProperty("relation")]
    public List<Relation> Relation { get; set; } = default!;
}