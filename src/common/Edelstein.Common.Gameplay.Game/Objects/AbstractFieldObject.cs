using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Objects;

public abstract class AbstractFieldObject : IFieldObject
{

    protected AbstractFieldObject(IPoint2D position) => Position = position;

    private bool IsHidden { get; set; }
    public abstract FieldObjectType Type { get; }

    public int? ObjectID { get; set; }

    public IField? Field { get; set; }
    public IFieldSplit? FieldSplit { get; set; }
    public IPoint2D Position { get; protected set; }

    public bool IsVisibleTo(IFieldObject obj) => !IsHidden;

    public async Task Hide(bool hidden = true)
    {
        if (IsHidden == hidden) return;
        if (FieldSplit == null) return;

        if (hidden) await FieldSplit.Dispatch(GetLeaveFieldPacket(), this);
        IsHidden = hidden;
        if (!hidden) await FieldSplit.Dispatch(GetEnterFieldPacket(), this);

        if (this is not IFieldObjectController controller) return;

        foreach (var controlled in controller.Controlled.ToImmutableList())
            await controlled.Control();
    }

    public abstract IPacket GetEnterFieldPacket();
    public abstract IPacket GetLeaveFieldPacket();
}
