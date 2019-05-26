using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Interactions.Miniroom
{
    public abstract class AbstractMiniroom : IMiniroom
    {
        public abstract MiniroomType Type { get; }
        protected abstract byte MaxUsers { get; }

        protected IDictionary<byte, FieldUser> Users { get; }

        public AbstractMiniroom()
        {
            Users = new Dictionary<byte, FieldUser>();
        }

        public async Task<MiniroomEnterResult> Enter(FieldUser user)
        {
            return MiniroomEnterResult.Full;
        }

        public async Task Leave(FieldUser user, MiniroomLeaveResult leaveResult = MiniroomLeaveResult.UserRequest)
        {
        }

        public Task BroadcastPacket(IPacket packet)
            => BroadcastPacket(null, packet);

        public Task BroadcastPacket(FieldUser source, IPacket packet)
            => Task.WhenAll(
                Users.Values
                    .Where(u => u != source)
                    .Select(u => u.SendPacket(packet))
            );
    }
}