using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects;

public interface IFieldObject
{
    FieldObjectType Type { get; }

    int? ObjectID { get; set; }

    IField? Field { get; set; }
    IFieldSplit? FieldSplit { get; set; }
    IPoint2D Position { get; }

    bool IsVisibleTo(IFieldSplitObserver observer);

    Task Hide(bool hidden = true);

    IPacket GetEnterFieldPacket();
    IPacket GetLeaveFieldPacket();
}
