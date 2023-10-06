using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Templates;

public record MobStringTemplate : IMobStringTemplate
{
    public int ID { get; }
    public string Name { get; }
    
    public MobStringTemplate(int id, IDataProperty property)
    {
        ID = id;
        Name = property.ResolveOrDefault<string>("name") ?? string.Empty;
    }
}
