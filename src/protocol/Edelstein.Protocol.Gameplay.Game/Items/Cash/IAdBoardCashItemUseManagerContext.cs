using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Items.Cash;

public interface IAdBoardCashItemUseManagerContext : ICashItemUseManagerContext<IItemTemplate, StructuredAdBoardCashItemUseInfoEx>
{
    string Text { get; set; }
}
