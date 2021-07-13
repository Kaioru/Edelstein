using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects
{
    public interface IFieldObj
    {
        FieldObjType Type { get; }

        int ObjID { get; set; }

        IField Field { get; set; }
        IFieldSplit FieldSplit { get; set; }

        Point2D Position { get; set; }

        Task UpdateFieldSplit(
            Func<IPacket> getEnterPacket = null,
            Func<IPacket> getLeavePacket = null
        );

        IPacket GetEnterFieldPacket();
        IPacket GetLeaveFieldPacket();
    }
}
