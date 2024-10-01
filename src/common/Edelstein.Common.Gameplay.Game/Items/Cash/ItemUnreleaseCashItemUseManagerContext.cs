using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Items;
using Edelstein.Protocol.Gameplay.Game.Items.Cash;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game.Items.Cash;

public record ItemUnreleaseCashItemUseManagerContext(
    IFieldUser User, 
    ItemSlotBase Item, 
    IItemTemplate Template,
    ICashItemUseInfo Info, 
    StructuredItemUnreleaseCashItemUseInfoEx InfoEx
) : AbstractCashItemUseManagerContext<IItemTemplate, StructuredItemUnreleaseCashItemUseInfoEx>(User, Item, Template, Info, InfoEx), 
    IItemUnreleaseCashItemUseManagerContext;
