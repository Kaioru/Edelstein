using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC.Templates;

public record NPCStringTemplate : INPCStringTemplate
{
    public int ID { get; }
    public string Name { get; }
    public string Func { get; }

    public NPCStringTemplate(int id, IDataProperty property)
    {
        ID = id;
        Name = property.ResolveOrDefault<string>("name") ?? string.Empty;
        Func = property.ResolveOrDefault<string>("func") ?? string.Empty;
    }
}
