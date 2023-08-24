using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Generators;

public interface IFieldGeneratorUnit
{
    IPoint2D Position { get; }
    IFieldObject? Generate();
}
