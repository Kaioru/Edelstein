using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class UserChatHandler : AbstractFieldUserHandler
    {
        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            packet.Decode<int>();

            var message = packet.Decode<string>();
            var onlyBalloon = packet.Decode<bool>();

            using var p1 = new Packet(SendPacketOperations.UserChat);

            p1.Encode<int>(user.ID);
            p1.Encode<bool>(false);
            p1.Encode<string>(message);
            p1.Encode<bool>(onlyBalloon);

            await user.FieldSplit.BroadcastPacket(p1);

            using var p2 = new Packet(SendPacketOperations.UserChatNLCPQ);

            p2.Encode<int>(user.ID);
            p2.Encode<bool>(false);
            p2.Encode<string>(message);
            p2.Encode<bool>(onlyBalloon);
            p2.Encode<string>(user.Character.Name);

            await Task.WhenAll(user.Field
                .GetObjects<IFieldUser>()
                .Except(user.FieldSplit.GetWatchers())
                .Select(u => u.SendPacket(p2)));
        }
    }
}