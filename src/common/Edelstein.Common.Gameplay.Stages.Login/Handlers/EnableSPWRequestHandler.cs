using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Protocol.Interop.Contracts;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers
{
    public class EnableSPWRequestHandler : AbstractPacketHandler<LoginStage, LoginStageUser>
    {
        public override short Operation => (short)PacketRecvOperations.EnableSPWRequest;
        private readonly bool _isVAC;

        public EnableSPWRequestHandler(bool isVAC)
            => _isVAC = isVAC;

        public override Task<bool> Check(LoginStageUser user)
            => Task.FromResult(user.State == LoginState.SelectCharacter && user.Account.SPW == null);

        public override async Task Handle(LoginStageUser user, IPacketReader packet)
        {
            _ = packet.ReadBool(); // Unknown1
            var characterID = packet.ReadInt();

            if (_isVAC) _ = packet.ReadInt(); // Unknown2

            _ = packet.ReadString(); // sMacAddress
            _ = packet.ReadString(); // sMacAddressWithHDDSerial
            var spw = packet.ReadString();

            var result = LoginResultCode.Success;
            var gameServerRequest = new DescribeServersRequest();

            gameServerRequest.Tags.Add("Type", Enum.GetName(ServerStageType.Game));
            gameServerRequest.Tags.Add("WorldID", user.SelectedWorldID.ToString());
            gameServerRequest.Tags.Add("ChannelID", user.SelectedChannelID.ToString());

            var gameServers = (await user.Stage.ServerRegistryService.DescribeServers(gameServerRequest)).Servers;
            var gameServer = gameServers.FirstOrDefault();

            var character = await user.Stage.CharacterRepository.Retrieve(characterID);

            if (BCrypt.Net.BCrypt.EnhancedVerify(spw, user.Account.Password)) result = LoginResultCode.SamePasswordAndSPW;
            if (gameServer == null) result = LoginResultCode.NotConnectableWorld;
            if (character == null || character.AccountWorldID != user.AccountWorld.ID) result = LoginResultCode.DBFail;

            if (result != LoginResultCode.Success)
            {
                var response = new UnstructuredOutgoingPacket(PacketSendOperations.EnableSPWResult);

                response.WriteBool(false);
                response.WriteByte((byte)result);

                await user.Dispatch(response);
                return;
            }

            user.Character = character;
            user.Account.SPW = BCrypt.Net.BCrypt.EnhancedHashPassword(spw);
            await user.MigrateTo(gameServer);
        }
    }
}
