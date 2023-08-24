using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Templates;

namespace Edelstein.Common.Gameplay.Game.Templates;

public record FieldStringTemplate : IFieldStringTemplate
{
    public FieldStringTemplate(int id, IDataProperty property)
    {
        ID = id;

        MapName = property.ResolveOrDefault<string>("mapName") ?? string.Empty;
        StreetName = property.ResolveOrDefault<string>("streetName") ?? string.Empty;
    }
    
    public int ID { get; }
    
    public string MapName { get; }
    public string StreetName { get; }
}
