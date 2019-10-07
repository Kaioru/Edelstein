using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Extensions;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Interactions.Miniroom
{
    public abstract class AbstractMiniroom : IMiniroom
    {
        public abstract MiniroomType Type { get; }
        public abstract byte MaxUsers { get; }
        public IDictionary<byte, FieldUser> Users { get; }

        protected AbstractMiniroom()
        {
            Users = new Dictionary<byte, FieldUser>();
        }

        public async Task<MiniroomEnterResult> Enter(IMiniroomDialog dialog)
        {
            var id = byte.MaxValue;

            foreach (var i in Enumerable.Range(0, MaxUsers))
            {
                if (Users.ContainsKey((byte) i)) continue;
                id = (byte) i;
                break;
            }

            if (id >= MaxUsers) return MiniroomEnterResult.Full;
            if (Users.ContainsKey(id)) return MiniroomEnterResult.Etc;

            dialog.ID = id;
            Users[id] = dialog.User;

            using (var p = new Packet(SendPacketOperations.MiniRoom))
            {
                p.Encode<byte>((byte) MiniroomAction.MRP_EnterResult);
                p.Encode<byte>((byte) Type);
                p.Encode<byte>(MaxUsers);
                p.Encode<byte>(dialog.ID);

                Users.ForEach(kv =>
                {
                    var character = kv.Value.Character;

                    p.Encode<byte>(kv.Key);

                    character.EncodeLook(p);
                    p.Encode<string>(character.Name);
                    p.Encode<short>(character.Job);
                });
                p.Encode<byte>(0xFF);

                await dialog.User.SendPacket(p);
            }

            using (var p = new Packet(SendPacketOperations.MiniRoom))
            {
                var character = dialog.User.Character;

                p.Encode<byte>((byte) MiniroomAction.MRP_Enter);
                p.Encode<byte>(dialog.ID);

                character.EncodeLook(p);
                p.Encode<string>(character.Name);
                p.Encode<short>(character.Job);

                await BroadcastPacket(dialog.User, p);
            }

            return MiniroomEnterResult.Success;
        }

        public async Task Leave(IMiniroomDialog dialog,
            MiniroomLeaveResult leaveResult = MiniroomLeaveResult.UserRequest)
        {
            Users.Remove(dialog.ID);
            using var p = new Packet(SendPacketOperations.MiniRoom);
            p.Encode<byte>((byte) MiniroomAction.MRP_Leave);
            p.Encode<byte>(dialog.ID);
            p.Encode<byte>((byte) leaveResult);
            await BroadcastPacket(p);
        }

        public Task BroadcastPacket(IPacket packet)
            => BroadcastPacket(null, packet);

        public Task BroadcastPacket(FieldUser user, IPacket packet)
            => Task.WhenAll(
                Users.Values
                    .Where(u => u != user)
                    .Select(u => u.SendPacket(packet))
            );
    }
}