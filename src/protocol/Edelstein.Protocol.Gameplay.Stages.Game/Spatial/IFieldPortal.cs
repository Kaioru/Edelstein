using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Spatial;

public interface IFieldPortal : IFieldSpaceObject
{
    FieldPortalType Type { get; }

    string Name { get; }
    string? Script { get; }

    int ToMap { get; }
    string? ToName { get; }

    IPoint2D Position { get; }
}
