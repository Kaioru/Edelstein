using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC.Templates;

public record NPCStringTemplate : INPCStringTemplate
{
    public int ID { get; }
    public string Name { get; }
    public string Func { get; }

    public NPCStringTemplate(int id, IDataNode node)
    {
        ID = id;
        Name = node.ResolveString("name") ?? string.Empty;
        Func = node.ResolveString("func") ?? string.Empty;
    }
}
