using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Items;
using Edelstein.Protocol.Gameplay.Game.Items.Cash;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game.Items.Cash;

public record AdBoardCashItemUseManagerContext(
    IFieldUser User, 
    ItemSlotBase Item, 
    IItemTemplate Template,
    ICashItemUseInfo Info, 
    StructuredAdBoardCashItemUseInfoEx InfoEx
) : AbstractCashItemUseManagerContext<IItemTemplate, StructuredAdBoardCashItemUseInfoEx>(User, Item, Template, Info, InfoEx), 
    IAdBoardCashItemUseManagerContext
{
    public string Text { get; set; } = InfoEx.Text.Value;
}
