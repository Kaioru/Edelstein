using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class CharacterCreateCheckDuplicatedIDPlug : IPipelinePlug<ICharacterCreateCheckDuplicatedID>
{
    private readonly ICharacterRepository _characters;

    public CharacterCreateCheckDuplicatedIDPlug(ICharacterRepository characters) => _characters = characters;

    public async Task Handle(IPipelineContext ctx, ICharacterCreateCheckDuplicatedID message)
    {
        var result = await _characters.CheckExistsByName(message.Name);
        var packet = new PacketWriter(PacketSendOperations.CheckDuplicatedIDResult);

        packet.WriteString(message.Name);
        packet.WriteBool(result);

        await message.User.Dispatch(packet);
    }
}
