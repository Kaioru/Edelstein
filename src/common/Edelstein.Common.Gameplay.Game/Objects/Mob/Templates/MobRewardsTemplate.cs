using System.Collections.Immutable;
using Duey.Abstractions;
using Edelstein.Common.Gameplay.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Templates;

public class MobRewardsTemplate : ITemplate
{
    public int ID { get; }
    public ICollection<MobRewardsTemplateItem> Items { get; }
    
    public MobRewardsTemplate(int id, IDataNode property)
    {
        ID = id;
        Items = property.Children
            .Select(p => new MobRewardsTemplateItem(Convert.ToInt32(p.Name), p.Cache()))
            .ToImmutableArray();
    }
}
