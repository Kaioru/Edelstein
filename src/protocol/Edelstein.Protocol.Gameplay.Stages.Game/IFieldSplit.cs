using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Stages.Game
{
    public interface IFieldSplit : IFieldPool
    {
        int Row { get; }
        int Col { get; }

        Task Enter(IFieldObj obj, Func<IPacket> getEnterPacket = null, Func<IPacket> getLeavePacket = null);
        Task Leave(IFieldObj obj, Func<IPacket> getLeavePacket = null);

        Task EnterQuietly(IFieldObj obj);
        Task LeaveQuietly(IFieldObj obj);

        Task Watch(IFieldObjUser user);
        Task Unwatch(IFieldObjUser user);

        IFieldObjUser GetWatcher(int id);
        IEnumerable<IFieldObjUser> GetWatchers();
    }
}
