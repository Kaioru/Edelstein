using Edelstein.Protocol.Gameplay.Game.Continents;
using Edelstein.Protocol.Gameplay.Game.Items.Consume;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextManagers(
    IFieldManager Field,
    IFieldSetManager FieldSet,
    IContiMoveManager ContiMove,
    
    IStatChangeItemUseManager StatChangeItemUse
);
