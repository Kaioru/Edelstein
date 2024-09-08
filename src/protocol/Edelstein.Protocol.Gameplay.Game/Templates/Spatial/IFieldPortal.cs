using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Templates.Spatial;

public interface IFieldPortal : IFieldSpaceObject
{
    FieldPortalType Type { get; }

    string Name { get; }
    string? Script { get; }

    int ToMap { get; }
    string? ToName { get; }

    IPoint2D Position { get; }
}
