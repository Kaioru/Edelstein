using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob;

public interface IFieldMob : IFieldLife<IFieldMobMovePath, IFieldMobMoveAction>
{
    IMobTemplate Template { get; }
}
