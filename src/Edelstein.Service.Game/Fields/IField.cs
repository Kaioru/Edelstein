using System;
using System.Threading.Tasks;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Field;

namespace Edelstein.Service.Game.Fields
{
    public interface IField : IFieldPool
    {
        int ID { get; }

        FieldTemplate Template { get; }
        
        IFieldPool GetPool(FieldObjType type);

        Task Enter(IFieldObj obj, byte portal, Func<IPacket> getEnterPacket = null);
        Task Enter(IFieldObj obj, string portal, Func<IPacket> getEnterPacket = null);

        Task Enter(IFieldObj obj, Func<IPacket> getEnterPacket = null);
        Task Leave(IFieldObj obj, Func<IPacket> getLeavePacket = null);

        Task BroadcastPacket(IPacket packet);
        Task BroadcastPacket(IFieldObj source, IPacket packet);
    }
}