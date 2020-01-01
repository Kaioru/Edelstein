using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects;

namespace Edelstein.Service.Game.Fields
{
    public interface IFieldSplit : IFieldPool
    {
        ICollection<IFieldUser> Watchers { get; }

        int Row { get; }
        int Col { get; }
        
        Task Enter(IFieldObj obj, IFieldSplit from = null, Func<IPacket> getEnterPacket = null, Func<IPacket> getLeavePacket = null);
        Task Leave(IFieldObj obj, Func<IPacket> getLeavePacket = null);

        Task EnterQuietly(IFieldObj obj);
        Task LeaveQuietly(IFieldObj obj);

        Task Watch(IFieldUser user);
        Task Unwatch(IFieldUser user);
        
        IEnumerable<IFieldUser> GetWatchers();

        Task BroadcastPacket(IPacket packet);
        Task BroadcastPacket(IFieldObj source, IPacket packet);
    }
}