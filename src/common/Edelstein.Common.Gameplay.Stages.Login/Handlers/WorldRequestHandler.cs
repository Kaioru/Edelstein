using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Interop.Contracts;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers
{
    public class WorldRequestHandler : AbstractPacketHandler<LoginStage, LoginStageUser>
    {
        public override short Operation => (short)PacketRecvOperations.WorldRequest;

        public override Task<bool> Check(LoginStageUser user)
            => Task.FromResult(user.State == LoginState.SelectWorld);

        public override async Task Handle(LoginStageUser user, IPacketReader packet)
        {
            var worldTemplates = (await Task.WhenAll(user.Stage.Config.Worlds
                .Select(w => user.Stage.WorldTemplates.Retrieve(w))))
                .Where(w => w != null)
                .OrderBy(w => w.ID)
                .ToList();

            foreach (var world in worldTemplates)
            {
                var response = new UnstructuredOutgoingPacket(PacketSendOperations.WorldInformation);

                response.WriteByte((byte)world.ID);
                response.WriteString(world.Name);
                response.WriteByte(world.State);
                response.WriteString(""); // WorldEventDesc
                response.WriteShort(0); // WorldEventEXP_WSE, WorldSpecificEvent
                response.WriteShort(0); // WorldEventDrop_WSE, WorldSpecificEvent
                response.WriteBool(world.BlockCharCreation);

                var channelServerRequest = new DescribeServersRequest();

                channelServerRequest.Tags.Add("Type", Enum.GetName(ServerStageType.Game));
                channelServerRequest.Tags.Add("WorldID", world.ID.ToString());

                var channelServers = (await user.Stage.ServerRegistryService.DescribeServers(channelServerRequest)).Servers
                    .OrderBy(c => c.Id)
                    .ToList();

                response.WriteByte((byte)channelServers.Count);

                foreach (var channel in channelServers)
                {
                    response.WriteString(channel.Tags["ID"]);
                    response.WriteInt(0); // TODO: UserNo
                    response.WriteByte(Convert.ToByte(channel.Tags["WorldID"]));
                    response.WriteByte(Convert.ToByte(channel.Tags["ChannelID"]));
                    response.WriteBool(false); // TODO: AdultChannel
                }

                response.WriteShort(0); // TODO: Balloon
                await user.Dispatch(response);
            }

            await user.Dispatch(
                new UnstructuredOutgoingPacket(PacketSendOperations.WorldInformation)
                    .WriteByte(0xFF)
            );
            await user.Dispatch(
                new UnstructuredOutgoingPacket(PacketSendOperations.LatestConnectedWorld)
                    .WriteInt(user.Account.LatestConnectedWorld ?? 0)
            );
        }
    }
}
