using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Services.Contracts.Social;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers
{
    public class DeleteCharacterHandler : AbstractPacketHandler<LoginStage, LoginStageUser>
    {
        public override short Operation => (short)PacketRecvOperations.DeleteCharacter;

        public override Task<bool> Check(LoginStageUser user)
            => Task.FromResult(user.State == LoginState.SelectCharacter && user.Account.SPW != null);

        public override async Task Handle(LoginStageUser user, IPacketReader packet)
        {
            var spw = packet.ReadString();
            var characterID = packet.ReadInt();

            var result = LoginResultCode.Success;
            var response = new UnstructuredOutgoingPacket(PacketSendOperations.DeleteCharacterResult);
            var character = await user.Stage.CharacterRepository.Retrieve(characterID);

            if (!BCrypt.Net.BCrypt.EnhancedVerify(spw, user.Account.SPW)) result = LoginResultCode.IncorrectSPW;
            if (character == null || character.AccountWorldID != user.AccountWorld.ID) result = LoginResultCode.DBFail;

            var guild = (await user.Stage.GuildService.LoadByCharacter(new GuildLoadByCharacterRequest { Character = characterID })).Guild;
            var party = (await user.Stage.PartyService.LoadByCharacter(new PartyLoadByCharacterRequest { Character = characterID })).Party;

            if (result == LoginResultCode.Success)
            {
                // TODO: guild withdraw if not master

                if (party != null)
                    _ = user.Stage.PartyService.Withdraw(new PartyWithdrawRequest { Character = characterID });

                await user.Stage.CharacterRepository.Delete(characterID);
            }

            response.WriteInt(characterID);
            response.WriteByte((byte)result);

            await user.Dispatch(response);
        }
    }
}
