using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Objects;

public interface IFieldObject
{
    FieldObjectType Type { get; }
    
    int? ObjectID { get; set; }
    
    IField? Field { get; set; }
    IFieldSplit? FieldSplit { get; set; }
    IPoint2D Position { get; }

    bool IsVisibleTo(IFieldObject other);

    IDispatchable GetDispatchEnterField(bool isEnterField = false);
    IDispatchable GetDispatchLeaveField(bool isLeaveField = false);
}
