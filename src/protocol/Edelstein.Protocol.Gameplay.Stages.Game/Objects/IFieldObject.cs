using Edelstein.Protocol.Util.Buffers.Bytes;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects;

public interface IFieldObject
{
    FieldObjectType Type { get; }

    int? ObjectID { get; set; }

    IField? Field { get; }
    IPoint2D Position { get; }

    Task OnEnterField(int id, IField field);
    Task OnLeaveField();

    IPacket GetEnterFieldPacket();
    IPacket GetLeaveFieldPacket();
}
