using System;
using System.Threading.Tasks;
using Edelstein.Network.Packet;
using Edelstein.Provider.Templates.Field;

namespace Edelstein.Service.Game.Field
{
    public interface IField : IFieldPool
    {
        int ID { get; }
        FieldTemplate Template { get; }

        Task Enter(IFieldObj obj, Func<IPacket> getEnterPacket);
        Task Leave(IFieldObj obj, Func<IPacket> getLeavePacket);

        IFieldPool GetPool(FieldObjType type);

        Task BroadcastPacket(IPacket packet);
        Task BroadcastPacket(IFieldObj source, IPacket packet);
    }
}