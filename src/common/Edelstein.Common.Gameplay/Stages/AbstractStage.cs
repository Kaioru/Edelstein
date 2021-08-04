using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages
{
    public abstract class AbstractStage<TStage, TUser> : IStage<TStage, TUser>
        where TStage : AbstractStage<TStage, TUser>
        where TUser : AbstractStageUser<TStage, TUser>
    {
        private readonly IDictionary<int, TUser> _users;

        public AbstractStage()
        {
            _users = new Dictionary<int, TUser>();
        }

        public TUser GetUser(int id)
            => _users.ContainsKey(id) ? _users[id] : null;

        public IEnumerable<TUser> GetUsers()
            => _users.Values.ToImmutableList();

        public virtual Task Enter(TUser user)
        {
            user.Stage?.Leave(user);
            user.Stage = (TStage)this;
            _users.Add(user.ID, user);

            return Task.CompletedTask;
        }

        public virtual Task Leave(TUser user)
        {
            _users.Remove(user.ID);
            user.Stage = default;
            return Task.CompletedTask;
        }

        public Task Dispatch(IPacket packet)
            => Task.WhenAll(_users.Values.Select(user => user.Dispatch(packet)));

        public Task Dispatch(IEnumerable<IPacket> packets)
            => Task.WhenAll(_users.Values.Select(user => user.Dispatch(packets)));
    }
}
