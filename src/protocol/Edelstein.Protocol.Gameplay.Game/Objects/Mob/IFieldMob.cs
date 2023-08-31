using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob;

public interface IFieldMob : IFieldLife<IFieldMobMovePath, IFieldMobMoveAction>, IFieldControllable
{
    IMobTemplate Template { get; }
    
    IFieldMobStats Stats { get; }
    int HP { get; }
    int MP { get; }

    Task Damage(int damage, IFieldUser? attacker = null);
}
