using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Movements;

public interface IMovePath : IPacketReadable, IPacketWritable
{
    IPoint2D? Position { get; }
    int? FootholdID { get; }
}
