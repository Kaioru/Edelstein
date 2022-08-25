using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Templates;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob;

public interface IFieldMob : IFieldLife<IMobMovePath, IMobMoveAction>, IFieldControllable
{
    IMobTemplate Template { get; }
}
