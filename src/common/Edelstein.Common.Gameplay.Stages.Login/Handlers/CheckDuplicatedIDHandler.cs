using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class CheckDuplicatedIDHandler : AbstractLoginPacketHandler
{
    private readonly ICharacterRepository _characterRepository;

    public CheckDuplicatedIDHandler(ICharacterRepository characterRepository) =>
        _characterRepository = characterRepository;

    public override short Operation => (short)PacketRecvOperations.CheckDuplicatedID;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectCharacter;

    public override async Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var name = reader.ReadString();

        var result = await _characterRepository.CheckExistsByName(name);
        var packet = new PacketWriter();

        packet.WriteShort(10);

        packet.WriteString(name);
        packet.WriteBool(result);

        await user.Dispatch(packet);
    }
}
