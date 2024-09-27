using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Game.Templates.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob;

public interface IFieldMob : IFieldLife<IFieldMobMovePath, IFieldMobMoveAction>, IFieldObjectControllable
{
    IMobTemplate Template { get; }
    
    IFieldFoothold? FootholdStart { get; }
}
