using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Templates.Spatial;

public interface IFieldFoothold : IFieldSpaceObject
{
    int NextID { get; }
    int PrevID { get; }

    ILine2D Line { get; }
}
