using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Items;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game.Items;

public abstract record AbstractCashItemUseManagerContext<TTemplate, TInfoEx>(
    IFieldUser User,
    ItemSlotBase Item,
    TTemplate Template, 
    ICashItemUseInfo Info, 
    TInfoEx InfoEx
) : AbstractItemUseManagerContext<TTemplate, ICashItemUseInfo>(User, Item, Template, Info), 
    ICashItemUseManagerContext<TTemplate, TInfoEx>
    where TTemplate : IItemTemplate 
    where TInfoEx : ICashItemUseInfoEx;
