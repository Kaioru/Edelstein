using System.Drawing;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects
{
    public interface IFieldObj
    {
        int ID { get; set; }
        FieldObjType Type { get; }
        
        IField Field { get; set; }

        Point Position { get; set; }

        IPacket GetEnterFieldPacket();
        IPacket GetLeaveFieldPacket();
    }
}