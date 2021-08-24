using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Gameplay.Users;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Services.Contracts;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers
{
    public class SelectWorldHandler : AbstractPacketHandler<LoginStage, LoginStageUser>
    {
        public override short Operation => (short)PacketRecvOperations.SelectWorld;

        public override Task<bool> Check(LoginStageUser user)
            => Task.FromResult(user.State == LoginState.SelectWorld);

        public override async Task Handle(LoginStageUser user, IPacketReader packet)
        {
            _ = packet.ReadByte(); // Unknown1

            var worldID = packet.ReadByte();
            var channelID = packet.ReadByte();

            var result = LoginResultCode.Success;
            var response = new UnstructuredOutgoingPacket(PacketSendOperations.SelectWorldResult);

            var gameServerRequest = new DescribeServerByMetadataRequest();

            gameServerRequest.Metadata.Add("Type", Enum.GetName(ServerStageType.Game));
            gameServerRequest.Metadata.Add("WorldID", worldID.ToString());

            var gameServers = (await user.Stage.ServerRegistry.DescribeByMetadata(gameServerRequest)).Servers
                .OrderBy(g => g.Metadata["ChannelID"])
                .ToList();

            if (channelID > gameServers.Count) result = LoginResultCode.NotConnectableWorld;

            response.WriteByte((byte)result);

            if (result == LoginResultCode.Success)
            {
                var gameServer = gameServers[channelID];
                var accountWorld = await user.Stage.AccountWorldRepository.RetrieveByAccountAndWorld(user.Account.ID, worldID);

                if (accountWorld == null)
                {
                    accountWorld = new AccountWorld
                    {
                        AccountID = user.Account.ID,
                        WorldID = worldID
                    };

                    await user.Stage.AccountWorldRepository.Insert(accountWorld);
                }

                user.State = LoginState.SelectCharacter;
                user.AccountWorld = accountWorld;
                user.SelectedWorldID = Convert.ToByte(gameServer.Metadata["WorldID"]);
                user.SelectedChannelID = Convert.ToByte(gameServer.Metadata["ChannelID"]);

                var characters = (await user.Stage.CharacterRepository.RetrieveAllByAccountWorld(accountWorld.ID)).ToList();

                response.WriteByte((byte)characters.Count);
                characters.ForEach(c =>
                {
                    response.WriteCharacterStats(c);
                    response.WriteCharacterLook(c);

                    response.WriteBool(false);
                    response.WriteBool(false);
                });

                response.WriteBool(!string.IsNullOrEmpty(user.Account.SPW)); // bLoginOpt TODO: proper bLoginOpt stuff
                response.WriteInt(accountWorld.SlotCount);
                response.WriteInt(0);
            }

            await user.Dispatch(response);
        }
    }
}
