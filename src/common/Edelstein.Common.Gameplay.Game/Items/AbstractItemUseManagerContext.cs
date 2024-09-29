using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Items;

namespace Edelstein.Common.Gameplay.Game.Items;

public abstract record AbstractItemUseManagerContext<TTemplate, TInfo>(
    ItemSlotBase Item,
    TTemplate Template,
    TInfo Info
) : IItemUseManagerContext<TTemplate, TInfo> 
    where TTemplate : IItemTemplate 
    where TInfo : IItemUseInfo
{
    public bool SkipHandle { get; set; }
    public bool SkipConsumption { get; set; }
}
