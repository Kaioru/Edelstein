using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Templates;

public interface IFieldTemplatePortal : IFieldSpaceObject
{
    FieldPortalType Type { get; }

    string Name { get; }
    string? Script { get; }

    int ToMap { get; }
    string? ToName { get; }

    IPoint2D Position { get; }
}
