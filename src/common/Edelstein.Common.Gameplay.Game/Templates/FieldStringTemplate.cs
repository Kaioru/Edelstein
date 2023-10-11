using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Templates;

namespace Edelstein.Common.Gameplay.Game.Templates;

public record FieldStringTemplate : IFieldStringTemplate
{
    public FieldStringTemplate(int id, IDataNode node)
    {
        ID = id;

        MapName = node.ResolveString("mapName") ?? string.Empty;
        StreetName = node.ResolveString("streetName") ?? string.Empty;
    }
    
    public int ID { get; }
    
    public string MapName { get; }
    public string StreetName { get; }
}
