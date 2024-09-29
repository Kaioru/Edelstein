using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Items;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game.Items;

public abstract record AbstractItemUseManagerContext<TTemplate, TInfo>(
    IFieldUser User,
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
