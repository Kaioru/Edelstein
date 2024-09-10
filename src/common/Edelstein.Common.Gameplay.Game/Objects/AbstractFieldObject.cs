using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Objects;

public abstract class AbstractFieldObject(
    IPoint2D position
) : IFieldObject
{
    public abstract FieldObjectType Type { get; }
    
    public int? ObjectID { get; set; }
    
    public IField? Field { get; set; }
    public IFieldSplit? FieldSplit { get; set; }
    public IPoint2D Position { get; } = position;

    public bool IsVisibleTo(IFieldObject other) => true;

    public abstract IDispatchable GetDispatchEnter(bool isEnterField = false);
    public abstract IDispatchable GetDispatchLeave(bool isLeaveField = false);
}
