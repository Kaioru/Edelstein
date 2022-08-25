using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Spatial;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob;

public interface IFieldMobFactory
{
    IFieldMob CreateMob(
        IMobTemplate template,
        IPoint2D position,
        IFieldFoothold? foothold = null,
        bool isFacingLeft = true
    );
}
