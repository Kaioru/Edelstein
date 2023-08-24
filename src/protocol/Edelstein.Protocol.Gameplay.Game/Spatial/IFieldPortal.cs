using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Spatial;

public interface IFieldPortal : IFieldSpaceObject
{
    FieldPortalType Type { get; }

    string Name { get; }
    string? Script { get; }

    int ToMap { get; }
    string? ToName { get; }

    IPoint2D Position { get; }
}
