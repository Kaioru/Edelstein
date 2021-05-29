using System;
using System.Numerics;
using System.Threading.Tasks;
using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects
{
    public interface IFieldObj
    {
        FieldObjType Type { get; }

        int ObjID { get; set; }

        IField Field { get; set; }
        IFieldSplit FieldSplit { get; set; }

        Vector2 Position { get; set; }

        Task UpdateFieldSplit(
            Func<IPacket> getEnterPacket = null,
            Func<IPacket> getLeavePacket = null
        );

        IPacket GetEnterFieldPacket();
        IPacket GetLeaveFieldPacket();
    }
}
