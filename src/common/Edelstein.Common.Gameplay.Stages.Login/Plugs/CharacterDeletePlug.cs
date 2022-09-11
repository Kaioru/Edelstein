using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class CharacterDeletePlug : IPipelinePlug<ICharacterDelete>
{
    private readonly ICharacterRepository _characters;

    public CharacterDeletePlug(ICharacterRepository characters) => _characters = characters;

    public async Task Handle(IPipelineContext ctx, ICharacterDelete message)
    {
        try
        {
            var result = LoginResult.Success;
            var packet = new PacketWriter(PacketSendOperations.DeleteCharacterResult);

            var character = await _characters.RetrieveByAccountWorldAndCharacter(
                message.User.AccountWorld!.ID,
                message.CharacterID
            );

            if (character == null) result = LoginResult.NotAuthorized;
            if (!BCrypt.Net.BCrypt.Verify(message.SPW, message.User.Account!.SPW)) result = LoginResult.IncorrectSPW;

            if (result == LoginResult.Success)
                await _characters.Delete(character!);

            packet.WriteInt(message.CharacterID);
            packet.WriteByte((byte)result);

            await message.User.Dispatch(packet);
        }
        catch (Exception)
        {
            await message.User.Dispatch(new PacketWriter(PacketSendOperations.DeleteCharacterResult)
                .WriteInt(message.CharacterID)
                .WriteByte((byte)LoginResult.DBFail)
            );
        }
    }
}
