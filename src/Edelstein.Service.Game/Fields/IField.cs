using System;
using System.Drawing;
using System.Threading.Tasks;
using Edelstein.Core.Templates.Fields;
using Edelstein.Core.Utils.Ticks;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects;

namespace Edelstein.Service.Game.Fields
{
    public interface IField : IFieldPool, ITickBehavior
    {
        FieldTemplate Template { get; }

        IFieldSplit GetSplit(Point position);
        IFieldSplit[] GetEnclosingSplits(Point position);
        IFieldSplit[] GetEnclosingSplits(IFieldSplit split);

        IFieldPool GetPool(FieldObjType type);
        IFieldPortal GetPortal(byte portal);
        IFieldPortal GetPortal(string portal);

        Task Enter(IFieldUser user, byte portal, Func<IPacket> getEnterPacket = null);
        Task Enter(IFieldUser user, string portal, Func<IPacket> getEnterPacket = null);

        Task Enter(IFieldObj obj, Func<IPacket> getEnterPacket = null);
        Task Leave(IFieldObj obj, Func<IPacket> getLeavePacket = null);

        Task BroadcastPacket(IPacket packet);
        Task BroadcastPacket(IFieldObj source, IPacket packet);
    }
}