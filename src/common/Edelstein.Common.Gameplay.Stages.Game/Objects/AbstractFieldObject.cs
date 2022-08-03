using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects;

public abstract class AbstractFieldObject : IFieldObject
{
    protected AbstractFieldObject(IPoint2D position) => Position = position;

    public abstract FieldObjectType Type { get; }

    public int? ObjectID { get; set; }

    public IField? Field { get; set; }
    public IFieldSplit? FieldSplit { get; set; }
    public IPoint2D Position { get; protected set; }

    public abstract IPacket GetEnterFieldPacket();
    public abstract IPacket GetLeaveFieldPacket();
}
