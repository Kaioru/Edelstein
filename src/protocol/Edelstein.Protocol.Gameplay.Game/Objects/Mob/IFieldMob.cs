using Edelstein.Protocol.Gameplay.Game.Combat.Damage;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats.Modify;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Spatial;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob;

public interface IFieldMob : IFieldLife<IFieldMobMovePath, IFieldMobMoveAction>, IFieldObjectControllable
{
    IMobTemplate Template { get; }
    
    IFieldFoothold? FootholdHome { get; }
    
    IFieldMobStats Stats { get; }
    IMobTemporaryStats TemporaryStats { get; }
    
    int HP { get; }
    int MP { get; }

    Task Damage(int damage, IFieldUser? attacker = null, IPoint2D? positionHit = null);
    
    Task ModifyTemporaryStats(Action<IModifyMobTemporaryStatContext> action = null);
}
