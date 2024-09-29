using Edelstein.Protocol.Gameplay.Game.Continents;
using Edelstein.Protocol.Gameplay.Game.Items.Cash;
using Edelstein.Protocol.Gameplay.Game.Items.Consume;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextManagers(
    IFieldManager Field,
    IFieldSetManager FieldSet,
    IContiMoveManager ContiMove,
    
    IStatChangeItemUseManager ItemUseStatChange,
    
    IAdBoardCashItemUseManager CashItemUseAdBoard
);
