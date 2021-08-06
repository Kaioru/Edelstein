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

                response.WriteByte((byte)world.ID);                // nWorldID
                response.WriteString(world.Name);                  // sName
                response.WriteByte(world.State);                   // nWorldState, 1 = Event, 2 = New, 3 = Hot
                response.WriteString(world.WorldEventDescription); // sWorldEventDesc
                response.WriteShort(world.WorldEventEXP);          // nWorldEventEXP_WSE, WorldSpecificEvent
                response.WriteShort(world.WorldEventDrop);         // nWorldEventDrop_WSE, WorldSpecificEvent
                response.WriteBool(world.BlockCharCreation);       // nBlockCharCreation 

                var channelServerRequest = new DescribeServersRequest();

                channelServerRequest.Tags.Add("Type", "Game");
                channelServerRequest.Tags.Add("WorldID", world.ID.ToString());

                var channelServers = (await user.Stage.ServerRegistryService.DescribeServers(channelServerRequest)).Servers
                    .OrderBy(c => c.Id)
                    .ToList();

                response.WriteByte((byte)channelServers.Count);

                foreach (var channel in channelServers)
                {
                    response.WriteString(channel.Tags["ID"]);                      // sName
                    response.WriteInt(0);                                          // TODO: nUserNo
                    response.WriteByte(Convert.ToByte(channel.Tags["WorldID"]));   // nWorldID
                    response.WriteByte(Convert.ToByte(channel.Tags["ChannelID"])); // nChannelID
                    response.WriteBool(false);                                     // TODO: bAdultChannel
                }

                response.WriteShort(0); // TODO: m_nBalloonCount

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
