using System;
using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Network.Packet;
using Edelstein.Provider.Templates.Field;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Fields
{
    public interface IField : IFieldPool, IUpdateable
    {
        int ID { get; }
        FieldTemplate Template { get; }

        Task Enter(FieldUser user, byte portal);
        Task Enter(FieldUser user, string portal);

        Task Enter(IFieldObj obj, Func<IPacket> getEnterPacket);
        Task Leave(IFieldObj obj, Func<IPacket> getLeavePacket);

        IFieldPool GetPool(FieldObjType type);

        Task BroadcastPacket(IPacket packet);
        Task BroadcastPacket(IFieldObj source, IPacket packet);
    }
}