using Edelstein.Protocol.Gameplay.Game.Continents;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextManagers(
    IFieldManager Field,
    IFieldSetManager FieldSet,
    IContiMoveManager ContiMove
);
