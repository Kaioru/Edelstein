using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Spatial;

public interface IFieldFoothold : IFieldSpaceObject
{
    int NextID { get; }
    int PrevID { get; }

    ISegment2D Line { get; }
}
