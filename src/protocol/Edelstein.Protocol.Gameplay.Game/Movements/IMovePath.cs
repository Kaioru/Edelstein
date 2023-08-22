using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Movements;

public interface IMovePath<out TMoveAction> : IPacketReadable, IPacketWritable
    where TMoveAction : IMoveAction
{
    TMoveAction? Action { get; }
    IPoint2D? Position { get; }
    int? FootholdID { get; }
}
