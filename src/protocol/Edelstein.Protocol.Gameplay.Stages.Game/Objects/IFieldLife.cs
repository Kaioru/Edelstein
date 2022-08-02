using Edelstein.Protocol.Gameplay.Stages.Game.Spatial;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects;

public interface IFieldLife : IFieldObject
{
    IFieldFoothold Foothold { get; }

    Task Move(IPoint2D position);
}
