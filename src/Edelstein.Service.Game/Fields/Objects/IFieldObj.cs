using System;
using System.Drawing;
using System.Threading.Tasks;
using Edelstein.Core.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects
{
    public interface IFieldObj
    {
        int ID { get; set; }
        FieldObjType Type { get; }

        IField Field { get; set; }

        IFieldSplit FieldSplit { get; set; }

        Point Position { get; set; }

        Task UpdateFieldSplit(
            Func<IPacket> getEnterPacket = null,
            Func<IPacket> getLeavePacket = null
        );

        IPacket GetEnterFieldPacket();
        IPacket GetLeaveFieldPacket();

        Task BroadcastPacket(IPacket packet);
    }
}