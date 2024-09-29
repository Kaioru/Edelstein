using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Items;

namespace Edelstein.Common.Gameplay.Game.Items;

public abstract record AbstractItemUseManagerContext<TTemplate>(
    TTemplate Template
) : IItemUseManagerContext<TTemplate> 
    where TTemplate : IItemTemplate
{
    public bool SkipHandle { get; set; }
    public bool SkipConsumption { get; set; }
}
