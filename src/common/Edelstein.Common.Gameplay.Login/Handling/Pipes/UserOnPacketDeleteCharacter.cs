using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Send;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Pipes;

public class UserOnPacketDeleteCharacter(
    ICharacterRepository characters
) : IPipe<PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, DeleteCharacter>>
{
    public async Task Handle(IPipelineContext ctx, PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, DeleteCharacter> message)
    {
        if (message.User.State != LoginState.SelectCharacter) return;
        if (string.IsNullOrEmpty(message.User.Account!.SPW)) return;

        try
        {
            if (!BCrypt.Net.BCrypt.EnhancedVerify(message.Packet.SPW.Value, message.User.Account!.SPW))
            {
                await message.User.Dispatch(new DeleteCharacterResult
                {
                    CharacterID = message.Packet.CharacterID,
                    Result = LoginSPWResultCode.IncorrectSPW
                });
                return;
            }
            
            var character = await characters.RetrieveByAccountWorldDataAndCharacter(
                message.User.AccountWorldData!.ID,
                message.Packet.CharacterID
            );

            await characters.Delete(character!);
            await message.User.Dispatch(new DeleteCharacterResult
            {
                CharacterID = character!.ID,
                Result = LoginSPWResultCode.Success
            });
        }
        catch
        {
            await message.User.Dispatch(new DeleteCharacterResult
            {
                CharacterID = message.Packet.CharacterID,
                Result = LoginSPWResultCode.Unknown
            });
        }
    }
}
