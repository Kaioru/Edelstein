using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects;

public abstract class AbstractFieldObject : IFieldObject
{
    protected AbstractFieldObject(IPoint2D position) => Position = position;
    private bool IsHidden { get; set; }

    public abstract FieldObjectType Type { get; }

    public int? ObjectID { get; set; }

    public IField? Field { get; set; }
    public IFieldSplit? FieldSplit { get; set; }
    public IPoint2D Position { get; protected set; }

    public bool IsVisibleTo(IFieldSplitObserver observer) => !IsHidden;

    public async Task Hide(bool hidden = true)
    {
        if (IsHidden == hidden) return;

        IsHidden = hidden;

        if (FieldSplit == null) return;
        if (IsHidden) await FieldSplit.Dispatch(GetLeaveFieldPacket(), this);
        else await FieldSplit.Dispatch(GetEnterFieldPacket(), this);
    }

    public abstract IPacket GetEnterFieldPacket();
    public abstract IPacket GetLeaveFieldPacket();
}
