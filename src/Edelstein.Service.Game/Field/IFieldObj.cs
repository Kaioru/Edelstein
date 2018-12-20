using System.Drawing;
using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Field
{
    public interface IFieldObj
    {
        FieldObjType Type { get; }
        int ID { get; set; }
        IField Field { get; set; }

        Point Position { get; set; }

        IPacket GetEnterFieldPacket();
        IPacket GetLeaveFieldPacket();
    }
}