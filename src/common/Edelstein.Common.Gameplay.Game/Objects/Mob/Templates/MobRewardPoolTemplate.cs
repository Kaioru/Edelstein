using System.Collections.Immutable;
using Duey.Abstractions;
using Edelstein.Common.Gameplay.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Templates;

public class MobRewardPoolTemplate : ITemplate
{
    public int ID { get; }
    public ICollection<MobRewardTemplate> Items { get; }
    
    public MobRewardPoolTemplate(int id, IDataNode property)
    {
        ID = id;
        Items = property.Children
            .Select(p => new MobRewardTemplate(Convert.ToInt32(p.Name), p.Cache()))
            .ToImmutableArray();
    }
}
