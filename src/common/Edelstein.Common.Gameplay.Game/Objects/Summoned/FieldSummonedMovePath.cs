using Edelstein.Common.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;

namespace Edelstein.Common.Gameplay.Game.Objects.Summoned;

public class FieldSummonedMovePath : AbstractMovePath<IFieldSummonedMoveAction>, IFieldSummonedMovePath
{
    protected override IFieldSummonedMoveAction GetActionFromRaw(byte raw)
        => new FieldSummonedMoveAction(raw);
}
