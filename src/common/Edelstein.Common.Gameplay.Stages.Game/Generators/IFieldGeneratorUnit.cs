using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Generators;

public interface IFieldGeneratorUnit
{
    IPoint2D Position { get; }
    IFieldObject? Generate();
}
