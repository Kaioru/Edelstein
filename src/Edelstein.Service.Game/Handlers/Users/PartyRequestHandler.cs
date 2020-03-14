using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Social.Party;
using Edelstein.Core.Utils;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Logging;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class PartyRequestHandler : AbstractFieldUserHandler
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            var type = (PartyRequestType) packet.Decode<byte>();

            switch (type)
            {
                default:
                    Logger.Warn($"Unhandled party request type: {type}");
                    break;
            }
        }
    }
}