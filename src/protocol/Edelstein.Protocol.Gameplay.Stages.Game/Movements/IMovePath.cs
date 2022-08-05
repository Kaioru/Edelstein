using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Movements;

public interface IMovePath<out TMoveAction> : IPacketReadable, IPacketWritable
    where TMoveAction : IMoveAction
{
    TMoveAction? Action { get; }
    IPoint2D? Position { get; }
    int? Foothold { get; }
}
