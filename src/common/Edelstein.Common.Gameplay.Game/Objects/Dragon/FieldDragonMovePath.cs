using Edelstein.Common.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Objects.Dragon;

namespace Edelstein.Common.Gameplay.Game.Objects.Dragon;

public class FieldDragonMovePath : AbstractMovePath<IFieldDragonMoveAction>, IFieldDragonMovePath
{
    protected override IFieldDragonMoveAction GetActionFromRaw(byte raw)
        => new FieldDragonMoveAction(raw);
}
