using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Social.Guild;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Logging;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class GuildRequestHandler : AbstractFieldUserHandler
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            var type = (GuildRequestType) packet.Decode<byte>();

            switch (type)
            {
                case GuildRequestType.LoadGuild:
                {
                    using var p = new Packet(SendPacketOperations.GuildResult);
                    p.Encode<byte>((byte) GuildResultType.LoadGuild_Done);

                    if (user.Guild != null)
                    {
                        p.Encode<bool>(true);
                        user.Guild.EncodeData(p);
                    }
                    else p.Encode<bool>(false);

                    await user.SendPacket(p);
                    break;
                }
                default:
                    Logger.Warn($"Unhandled guild request type: {type}");
                    break;
            }
        }
    }
}