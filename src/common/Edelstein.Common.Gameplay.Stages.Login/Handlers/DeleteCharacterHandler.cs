using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class DeleteCharacterHandler : AbstractLoginPacketHandler
{
    private readonly ICharacterRepository _characterRepository;

    public DeleteCharacterHandler(ICharacterRepository characterRepository) =>
        _characterRepository = characterRepository;

    public override short Operation => (short)PacketRecvOperations.DeleteCharacter;

    public override bool Check(ILoginStageUser user) =>
        user.State == LoginState.SelectCharacter && user.Account!.SPW != null;

    public override async Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var spw = reader.ReadString();
        var characterID = reader.ReadInt();

        try
        {
            var result = LoginResult.Success;
            var packet = new PacketWriter();

            packet.WriteShort(12);

            var character = await _characterRepository.RetrieveByAccountWorldAndCharacter(
                user.AccountWorld!.ID,
                characterID
            );

            if (character == null) result = LoginResult.NotAuthorized;
            if (!BCrypt.Net.BCrypt.Verify(spw, user.Account!.SPW)) result = LoginResult.IncorrectSPW;

            if (result == LoginResult.Success)
                await _characterRepository.Delete(character!);

            packet.WriteInt(characterID);
            packet.WriteByte((byte)result);

            await user.Dispatch(packet);
        }
        catch (Exception)
        {
            await user.Dispatch(new PacketWriter()
                .WriteShort(12)
                .WriteInt(characterID)
                .WriteByte((byte)LoginResult.DBFail)
            );
        }
    }
}
