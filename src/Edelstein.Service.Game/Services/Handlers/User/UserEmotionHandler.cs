using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers.User
{
    public class UserEmotionHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            var emotion = packet.Decode<int>();
            var duration = packet.Decode<int>();
            var byItemOption = packet.Decode<bool>();

            // TODO: item option checks

            using (var p = new Packet(SendPacketOperations.UserEmotion))
            {
                p.Encode<int>(user.ID);
                p.Encode<int>(emotion);
                p.Encode<int>(duration);
                p.Encode<bool>(byItemOption);
                await user.Field.BroadcastPacket(user, p);
            }
        }
    }
}