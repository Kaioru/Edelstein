using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Templates;

public record MobStringTemplate : IMobStringTemplate
{
    public int ID { get; }
    public string Name { get; }
    
    public MobStringTemplate(int id, IDataNode node)
    {
        ID = id;
        Name = node.ResolveString("name") ?? string.Empty;
    }
}
