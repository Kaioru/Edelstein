using System;
using System.Threading.Tasks;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects;

namespace Edelstein.Service.Game.Fields
{
    public interface IFieldSplit : IFieldPool
    {
        int Row { get; }
        int Col { get; }

        Task Enter(IFieldObj obj, Func<IPacket> getEnterPacket = null);
        Task Leave(IFieldObj obj, Func<IPacket> getLeavePacket = null);
    }
}