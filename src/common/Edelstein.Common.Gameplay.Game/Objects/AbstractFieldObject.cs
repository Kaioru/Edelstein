using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.AffectedArea;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Objects;

public abstract class AbstractFieldObject : IFieldObject
{
    protected AbstractFieldObject(IPoint2D position)
    {
        Position = position;
        Affected = new List<IFieldAffectedArea>();
    }

    private bool IsHidden { get; set; }
    public abstract FieldObjectType Type { get; }

    public int? ObjectID { get; set; }

    public IField? Field { get; set; }
    public IFieldSplit? FieldSplit { get; set; }
    public IPoint2D Position { get; protected set; }
    
    public ICollection<IFieldAffectedArea> Affected { get; }

    public bool IsVisibleTo(IFieldObject obj) => !IsHidden;
    

    private async Task UpdateFieldSplit()
    {
        var split = Field?.GetSplit(Position);

        if (split == null)
        {
            if (Field != null) await Field.Enter(this);
            return;
        }

        if (FieldSplit != split) await split.Enter(this);
    }
    
    public async Task Hide(bool hidden = true)
    {
        if (IsHidden == hidden) return;
        if (FieldSplit == null) return;

        if (hidden) await FieldSplit.Dispatch(GetLeaveFieldPacket(), this);
        IsHidden = hidden;
        if (!hidden) await FieldSplit.Dispatch(GetEnterFieldPacket(), this);

        if (this is not IFieldObjectController controller) return;

        foreach (var controlled in controller.Controlled.ToImmutableArray())
            await controlled.Control();
    }

    public abstract IPacket GetEnterFieldPacket();
    public abstract IPacket GetLeaveFieldPacket();
}
