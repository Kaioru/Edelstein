using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Items;

namespace Edelstein.Common.Gameplay.Game.Items;

public abstract record AbstractCashItemUseManagerContext<TTemplate, TInfo, TInfoEx>(
    ItemSlotBase Item,
    TTemplate Template, 
    TInfo Info, 
    TInfoEx InfoEx
) : AbstractItemUseManagerContext<TTemplate, TInfo>(Item, Template, Info), 
    ICashItemUseManagerContext<TTemplate, TInfo, TInfoEx>
    where TTemplate : IItemTemplate 
    where TInfo : ICashItemUseInfo 
    where TInfoEx : ICashItemUseInfoEx;
