using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Items;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game.Items;

public abstract record AbstractCashItemUseManagerContext<TTemplate, TInfo, TInfoEx>(
    IFieldUser User,
    ItemSlotBase Item,
    TTemplate Template, 
    TInfo Info, 
    TInfoEx InfoEx
) : AbstractItemUseManagerContext<TTemplate, TInfo>(User, Item, Template, Info), 
    ICashItemUseManagerContext<TTemplate, TInfo, TInfoEx>
    where TTemplate : IItemTemplate 
    where TInfo : ICashItemUseInfo 
    where TInfoEx : ICashItemUseInfoEx;
