using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Login.Types;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Plugs;

public class UserOnPacketDeleteCharacterPlug : IPipelinePlug<UserOnPacketDeleteCharacter>
{
    private readonly ICharacterRepository _characterRepository;

    public UserOnPacketDeleteCharacterPlug(ICharacterRepository characterRepository) => _characterRepository = characterRepository;

    public async Task Handle(IPipelineContext ctx, UserOnPacketDeleteCharacter message)
    {
        try
        {
            var result = LoginResult.Success;
            using var packet = new PacketWriter(PacketSendOperations.DeleteCharacterResult);

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

            await message.User.Dispatch(packet.Build());
        }
        catch (Exception)
        {
            using var failedPacket = new PacketWriter(PacketSendOperations.DeleteCharacterResult)
                .WriteInt(message.CharacterID)
                .WriteByte((byte)LoginResult.DBFail);
            await message.User.Dispatch(failedPacket.Build());
        }
    }
}
