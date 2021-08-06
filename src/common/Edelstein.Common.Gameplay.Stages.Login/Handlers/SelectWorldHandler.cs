using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Gameplay.Users;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Interop.Contracts;
using Edelstein.Protocol.Network;

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

            var gameServerRequest = new DescribeServersRequest();

            gameServerRequest.Tags.Add("Type", Enum.GetName(ServerStageType.Game));
            gameServerRequest.Tags.Add("WorldID", worldID.ToString());

            var gameServers = (await user.Stage.ServerRegistryService.DescribeServers(gameServerRequest)).Servers
                .OrderBy(g => g.Tags["ChannelID"])
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
                user.SelectedWorldID = Convert.ToByte(gameServer.Tags["WorldID"]);
                user.SelectedChannelID = Convert.ToByte(gameServer.Tags["ChannelID"]);
                user.Account.LatestConnectedWorld = worldID;

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
