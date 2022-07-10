using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Services.Contracts;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers
{
    public class CheckSPWRequestHandler : AbstractPacketHandler<LoginStage, LoginStageUser>
    {
        public override short Operation => (short)PacketRecvOperations.CheckSPWRequest;
        private readonly bool _isVAC;

        public CheckSPWRequestHandler(bool isVAC)
            => _isVAC = isVAC;

        public override Task<bool> Check(LoginStageUser user)
            => Task.FromResult(user.State == LoginState.SelectCharacter && user.Account.SPW != null);

        public override async Task Handle(LoginStageUser user, IPacketReader packet)
        {
            var spw = packet.ReadString();
            var characterID = packet.ReadInt();
            packet.ReadString(); // sMacAddress
            packet.ReadString(); // sMacAddressWithHDDSerial

            // TODO: ViewAllChar

            var result = LoginResultCode.Success;
            var gameServerRequest = new DescribeServerByMetadataRequest();

            gameServerRequest.Metadata.Add("Type", Enum.GetName(ServerStageType.Game));
            gameServerRequest.Metadata.Add("WorldID", user.SelectedWorldID.ToString());
            gameServerRequest.Metadata.Add("ChannelID", user.SelectedChannelID.ToString());

            var gameServers = (await user.Stage.ServerRegistry.DescribeByMetadata(gameServerRequest)).Servers;
            var gameServer = gameServers.FirstOrDefault();

            var character = await user.Stage.CharacterRepository.Retrieve(characterID);

            if (!BCrypt.Net.BCrypt.EnhancedVerify(spw, user.Account.SPW)) result = LoginResultCode.IncorrectSPW;
            if (gameServer == null) result = LoginResultCode.NotConnectableWorld;
            if (character == null || character.AccountWorldID != user.AccountWorld.ID) result = LoginResultCode.DBFail;

            if (result != LoginResultCode.Success)
            {
                var response = new UnstructuredOutgoingPacket(PacketSendOperations.CheckSPWResult);

                response.WriteBool(false);
                response.WriteByte((byte)result);

                await user.Dispatch(response);
                return;
            }

            user.Character = character;
            user.Account.LatestConnectedWorld = user.SelectedWorldID;
            await user.MigrateTo(gameServer);
        }
    }
}
