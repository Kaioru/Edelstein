using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class DeleteCharacterPlug : IPipelinePlug<IDeleteCharacter>
{
    private readonly ICharacterRepository _characterRepository;

    public DeleteCharacterPlug(ICharacterRepository characterRepository) => _characterRepository = characterRepository;

    public async Task Handle(IPipelineContext ctx, IDeleteCharacter message)
    {
        try
        {
            var result = LoginResult.Success;
            var packet = new PacketWriter(PacketSendOperations.DeleteCharacterResult);

            var character = await _characterRepository.RetrieveByAccountWorldAndCharacter(
                message.User.AccountWorld!.ID,
                message.CharacterID
            );

            if (character == null) result = LoginResult.NotAuthorized;
            if (!BCrypt.Net.BCrypt.Verify(message.SPW, message.User.Account!.SPW)) result = LoginResult.IncorrectSPW;

            if (result == LoginResult.Success)
                await _characterRepository.Delete(character!);

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
