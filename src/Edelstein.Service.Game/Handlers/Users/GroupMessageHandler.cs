using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Social;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Logging;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class GroupMessageHandler : AbstractFieldUserHandler
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacketDecoder packet
        )
        {
            packet.DecodeInt();

            var type = (GroupMessageType) packet.DecodeByte();
            var recipients = new int[packet.DecodeByte()];

            for (var i = 0; i < recipients.Length; i++)
                recipients[i] = packet.DecodeInt();

            var text = packet.DecodeString();

            switch (type)
            {
                case GroupMessageType.Party:
                    if (user.Party?.Members.Any(m => m.ChannelID >= 0) == true)
                        user.Party?.Chat(user.Character.Name, text);
                    break;
                case GroupMessageType.Guild:
                    if (user.Guild?.Members.Any(m => m.Online) == true)
                        user.Guild?.Chat(user.Character.Name, text);
                    break;
                default:
                    Logger.Warn($"Unhandled group message type: {type}");
                    break;
            }
        }
    }
}