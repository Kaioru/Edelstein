using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Services.Contracts;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class UserTransferChannelRequestHandler : AbstractUserPacketHandler
    {
        public override short Operation => (short)PacketRecvOperations.UserTransferChannelRequest;

        protected override async Task Handle(
            GameStageUser stageUser,
            IFieldObjUser user,
            IPacketReader packet
        )
        {
            var channelID = packet.ReadByte();

            // TODO checks, error handling

            var gameServerRequest = new DescribeServerByMetadataRequest();

            gameServerRequest.Metadata.Add("Type", Enum.GetName(ServerStageType.Game));
            gameServerRequest.Metadata.Add("WorldID", stageUser.Stage.WorldID.ToString());

            var gameServers = (await stageUser.Stage.ServerRegistry.DescribeByMetadata(gameServerRequest)).Servers
                .OrderBy(g => g.Metadata["ChannelID"])
                .ToList();
            var gameServer = gameServers[channelID];

            await stageUser.MigrateTo(gameServer);
        }
    }
}
