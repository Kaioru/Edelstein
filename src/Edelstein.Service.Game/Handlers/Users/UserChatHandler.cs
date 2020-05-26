using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Network.Packets;
using Edelstein.Core.Utils.Packets;
using Edelstein.Service.Game.Commands;
using Edelstein.Service.Game.Fields.Objects;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class UserChatHandler : AbstractFieldUserHandler
    {
        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacketDecoder packet
        )
        {
            packet.DecodeInt();

            var message = packet.DecodeString();
            var onlyBalloon = packet.DecodeBool();

            if (message.StartsWith(CommandManager.Prefix))
            {
                await user.Command(message.Substring(1));
                return;
            }

            using var p1 = new OutPacket(SendPacketOperations.UserChat);

            p1.EncodeInt(user.ID);
            p1.EncodeBool(false);
            p1.EncodeString(message);
            p1.EncodeBool(onlyBalloon);

            await user.FieldSplit.BroadcastPacket(p1);

            if (onlyBalloon) return;

            using var p2 = new OutPacket(SendPacketOperations.UserChatNLCPQ);

            p2.EncodeInt(user.ID);
            p2.EncodeBool(false);
            p2.EncodeString(message);
            p2.EncodeBool(onlyBalloon);
            p2.EncodeString(user.Character.Name);

            await Task.WhenAll(user.Field
                .GetObjects<IFieldUser>()
                .Except(user.FieldSplit.GetWatchers())
                .Select(u => u.SendPacket(p2)));
        }
    }
}