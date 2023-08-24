using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Spatial;

public interface IFieldFoothold : IFieldSpaceObject
{
    int NextID { get; }
    int PrevID { get; }

    ISegment2D Line { get; }
}
