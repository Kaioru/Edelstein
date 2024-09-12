using Edelstein.Common.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC;

public class FieldNPCMovePath : AbstractMovePath<IFieldNPCMoveAction>, IFieldNPCMovePath
{
    protected override IFieldNPCMoveAction GetActionFromValue(byte value) => new FieldNPCMoveAction(value);
}
